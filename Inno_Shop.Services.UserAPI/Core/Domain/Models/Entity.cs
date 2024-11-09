using System.Collections;
using System.Dynamic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Inno_Shop.Services.UserAPI.Core.Domain.LinkModels;

namespace Inno_Shop.Services.UserAPI.Core.Domain.Models;

public class Entity : DynamicObject, IXmlSerializable, IDictionary<string, object>
{
    private readonly string _root = "Entity";
    private readonly IDictionary<string, object> _expando;

    public Entity()
    {
        _expando = new ExpandoObject()!;
    }

    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        if (_expando.TryGetValue(binder.Name, out object? value))
        {
            result = value;
            return true;
        }
        return base.TryGetMember(binder, out result);
    }

    public override bool TrySetMember(SetMemberBinder binder, object? value)
    {
        ArgumentNullException.ThrowIfNull(value);
        _expando[binder.Name] = value;
        return true;
    }

    public XmlSchema GetSchema()
    {
        throw new NotImplementedException();
    }

    public void ReadXml(XmlReader reader)
    {
        reader.ReadStartElement(_root);

        while (!reader.Name.Equals(_root))
        {
            string typeContent;
            Type? underlyingType;
            var name = reader.Name;

            reader.MoveToAttribute("type");
            typeContent = reader.ReadContentAsString();
            underlyingType = Type.GetType(typeContent);
            reader.MoveToContent();
            ArgumentNullException.ThrowIfNull(underlyingType);
            _expando[name] = reader.ReadElementContentAs(underlyingType, null!);
        }
    }

    public void WriteXml(XmlWriter writer)
    {
        foreach (var key in _expando.Keys)
        {
            var value = _expando[key];
            WriteLinksToXml(key, value, writer);
        }
    }

    private void WriteLinksToXml(string key, object? value, XmlWriter writer) 
    { 
        writer.WriteStartElement(key); 
        ArgumentNullException.ThrowIfNull(value); 
        if (value is IList list) 
        { 
            foreach (var val in list) 
            { 
                if (val is Link link) 
                { 
                    writer.WriteStartElement(nameof(Link)); 
                    WriteLinksToXml(nameof(link.Href), link.Href!, writer); 
                    WriteLinksToXml(nameof(link.Method), link.Method!, writer); 
                    WriteLinksToXml(nameof(link.Rel), link.Rel!, writer); 
                    writer.WriteEndElement(); 
                } 
                else 
                { 
                    writer.WriteStartElement("Item"); 
                    writer.WriteString(val.ToString()); 
                    writer.WriteEndElement(); 
                } 
            } 
        } 
        else 
        { 
            writer.WriteString(value.ToString()); 
        } 
        writer.WriteEndElement(); 
    }

    public void Add(string key, object value)
    {
        _expando.Add(key, value);
    }

    public bool ContainsKey(string key)
    {
        return _expando.ContainsKey(key);
    }

    public ICollection<string> Keys
    {
        get { return _expando.Keys; }
    }

    public bool Remove(string key)
    {
        return _expando.Remove(key);
    }

    public bool TryGetValue(string key, out object value)
    {
        return _expando.TryGetValue(key, out value!);
    }

    public ICollection<object> Values
    {
        get { return _expando.Values; }
    }

    public object this[string key]
    {
        get
        {
            return _expando[key];
        }
        set
        {
            _expando[key] = value;
        }
    }

    public void Add(KeyValuePair<string, object> item)
    {
        _expando.Add(item);
    }

    public void Clear()
    {
        _expando.Clear();
    }

    public bool Contains(KeyValuePair<string, object> item)
    {
        return _expando.Contains(item);
    }

    public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
    {
        _expando.CopyTo(array, arrayIndex);
    }

    public int Count
    {
        get { return _expando.Count; }
    }

    public bool IsReadOnly
    {
        get { return _expando.IsReadOnly; }
    }

    public bool Remove(KeyValuePair<string, object> item)
    {
        return _expando.Remove(item);
    }

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
        return _expando.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}