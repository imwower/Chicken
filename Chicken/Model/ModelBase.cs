using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Chicken.Model
{
    public class ModelBase
    {
        [JsonIgnore]
        public bool HasError { get; set; }

        [JsonProperty("errors")]
        public List<ErrorMessage> Errors { get; set; }
    }

    public class ErrorMessage
    {
        public string Message { get; set; }

        public string Code { get; set; }
    }

    public class ModelBaseList<T> : ModelBase, IList<T>, ICollection<T>, IEnumerable<T>
        where T : ModelBase
    {
        private List<T> list { get; set; }

        public ModelBaseList()
        {
            list = new List<T>();
        }

        public void Add(T item)
        {
            list.Add(item);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int i = 0; i < list.Count; i++)
            {
                array[i] = (T)list[i];
            }
        }

        public int Count
        {
            get { return list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            return list.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                list[index] = value;
            }
        }
    }
}
