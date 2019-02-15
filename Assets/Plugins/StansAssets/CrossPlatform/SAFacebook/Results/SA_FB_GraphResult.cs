using System;
using System.Collections;
using UnityEngine;
using SA.Foundation.Templates;

namespace SA.Facebook
{
    /// <summary>
    /// Abstract Gprah API result class
    /// </summary>
    public abstract class SA_FB_GraphResult : SA_FB_Result
    {

        private string m_previous;
        private string m_next;

        private string m_before;
        private string m_after;

        private string m_id;


        public SA_FB_GraphResult(IGraphResult graphResult) : base(graphResult) {
           
        }


        protected void ParsePaginatedResult(IDictionary paginatedResult) {
            IDictionary paging = paginatedResult["paging"] as IDictionary;
            IDictionary cursors = paging["cursors"] as IDictionary;


            if (paging.Contains("previous")) {
                m_previous = Convert.ToString(paging["previous"]);
            }

            if (paging.Contains("next")) {
                m_next = Convert.ToString(paging["next"]);
            }



            m_before = Convert.ToString(cursors["before"]);
            m_after = Convert.ToString(cursors["after"]);
        }

        protected void ParseResultId(IDictionary rawDict)  {
            m_id = Convert.ToString(rawDict["id"]);
        }


        /// <summary>
        /// Request Id
        /// </summary>
        public string Id {
            get {
                return m_id;
            }
        }


        /// <summary>
        /// Full request URL for a next page
        /// </summary>
        public string Next {
            get {
                return m_next;
            }
        }


        /// <summary>
        /// True if request has next page of results
        /// </summary>
        public bool HasNext {
            get {
                return !string.IsNullOrEmpty(m_next);
            }
        }

        /// <summary>
        /// Full request URL for a previous page
        /// </summary>
        public string Previous {
            get {
                return m_previous;
            }
        }

        /// <summary>
        /// True if request has previous page of results
        /// </summary>
        public bool HasPrevious {
            get {
                return !string.IsNullOrEmpty(m_previous);
            }
        }

        /// <summary>
        /// Request before page pointer
        /// </summary>
        public string Before {
            get {
                return m_before;
            }
        }


        /// <summary>
        /// Request after page pointer
        /// </summary>
        public string After {
            get {
                return m_after;
            }
        }


        /// <summary>
        /// Generated before cursor pointer
        /// </summary>
        public SA_FB_Cursor BeforeCursorPointer {
            get {
                return new SA_FB_Cursor(SA_FB_CursorType.before, m_before);
            }
        }

        /// <summary>
        /// Generated after cursor pointer
        /// </summary>
        public SA_FB_Cursor AfterCursorPointer {
            get {
                return new SA_FB_Cursor(SA_FB_CursorType.after, m_after);
            }
        }



    }
}