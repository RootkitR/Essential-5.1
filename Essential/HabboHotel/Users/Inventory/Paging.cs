using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.HabboHotel.Users.Inventory
{
    public class PagingCollection<T> : IEnumerable<T>
    {
        #region fields
        private const int DefaultPageSize = 10;
        private readonly IEnumerable<T> _collection;
        private int _pageSize = DefaultPageSize;
        #endregion

        #region properties
        /// <summary>
        /// Gets or sets page size
        /// </summary>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException();
                }
                _pageSize = value;
            }
        }

        /// <summary>
        /// Gets pages count
        /// </summary>
        public int PagesCount
        {
            get
            {
                return (int)Math.Ceiling(_collection.Count() / (decimal)PageSize);
            }
        }
        #endregion

        #region ctor
        /// <summary>
        /// Creates paging collection and sets page size
        /// </summary>
        public PagingCollection(IEnumerable<T> collection, int pageSize)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            PageSize = pageSize;
            _collection = collection.ToArray();
        }

        /// <summary>
        /// Creates paging collection
        /// </summary>
        public PagingCollection(IEnumerable<T> collection)
            : this(collection, DefaultPageSize)
        { }
        #endregion

        #region public methods
        /// <summary>
        /// Returns data by page number
        /// </summary>
        public IEnumerable<T> GetData(int pageNumber)
        {
            if (pageNumber < 0 || pageNumber > PagesCount)
            {
                return new T[] { };
            }

            int offset = (pageNumber - 1) * PageSize;

            return _collection.Skip(offset).Take(PageSize);
        }

        /// <summary>
        /// Returns number of items on page by number
        /// </summary>
        public int GetCount(int pageNumber)
        {
            return GetData(pageNumber).Count();
        }
        #endregion

        #region static methods
        /// <summary>
        /// Returns data by page number and page size
        /// </summary>
        public static IEnumerable<T> GetPaging(IEnumerable<T> collection, int pageNumber, int pageSize)
        {
            return new PagingCollection<T>(collection, pageSize).GetData(pageNumber);
        }

        /// <summary>
        /// Returns data by page number
        /// </summary>
        public static IEnumerable<T> GetPaging(IEnumerable<T> collection, int pageNumber)
        {
            return new PagingCollection<T>(collection, DefaultPageSize).GetData(pageNumber);
        }
        #endregion

        #region IEnumerable<T> Members
        /// <summary>
        /// Returns an enumerator that iterates through collection
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }
        #endregion

        #region IEnumerable Members
        /// <summary>
        /// Returns an enumerator that iterates through collection
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
