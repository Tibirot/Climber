using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA.Foundation.Templates;

namespace SA.Facebook
{

    public class SA_FB_Result : SA_Result
    {
        private string m_rawResult = string.Empty;


        public SA_FB_Result(IResult graphResult) {
            m_error = GetResultError(graphResult);
            if (m_error == null) {
                m_rawResult = graphResult.RawResult;
            }
        }


        /// <summary>
        /// Gets the raw result string.
        /// </summary>
        public string RawResult {
            get {
                return m_rawResult;
            }
        }
        
        protected SA_Error GetResultError(IResult graphResult) {
            if (graphResult == null) {
                return SA_FB_ErrorFactory.GenerateErrorWithCode(SA_FB_ErrorCode.NullResult); 
            }

            if (graphResult.Cancelled) {
                return SA_FB_ErrorFactory.GenerateErrorWithCode(SA_FB_ErrorCode.UserCanceled);
            }

            if (!string.IsNullOrEmpty(graphResult.Error)) {
                return SA_FB_ErrorFactory.GenerateErrorWithCode(SA_FB_ErrorCode.APIError, graphResult.Error);  
            }

            if (string.IsNullOrEmpty(graphResult.RawResult)) {
                return SA_FB_ErrorFactory.GenerateErrorWithCode(SA_FB_ErrorCode.EmptyRawResult);
            }

            return null;
        }


    }
}
