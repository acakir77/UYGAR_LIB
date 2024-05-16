using System;
using System.Collections.Generic;
using System.Text;

namespace UYGAR.Exceptions
{
    public class ExceptionHelper
    {
        private Exception m_exception;

        public ExceptionHelper(Exception ex)
        {
            this.m_exception = ex;
        }

        public Type GetExceptionType()
        {
            return m_exception.GetType();
        }

        public String GetMessage()
        {
            return m_exception.Message;
        }

        public String GetDetailedMessage()
        {
            Exception ex = m_exception;

            StringBuilder errMessage = new StringBuilder();

            errMessage.Append(ex.Message).Append(Environment.NewLine).Append(ex.StackTrace);

            while (ex.InnerException != null)
            {
                errMessage.Append(BuildInnerExceptionString(ex.InnerException));
                ex = ex.InnerException;
            }

            return errMessage.ToString();
        }

        private String BuildInnerExceptionString(Exception innerException)
        {
            StringBuilder errMessage = new StringBuilder();

            errMessage.Append(Environment.NewLine).Append("InnerException ");
            errMessage.Append(Environment.NewLine).Append(innerException.Message).Append(Environment.NewLine).Append(innerException.StackTrace);

            return errMessage.ToString();
        }
    }
}
