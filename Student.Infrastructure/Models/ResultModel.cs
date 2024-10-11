namespace StudentApp.Infrastructure
{
    public class ResultModel<T>
    {
        #region constructor

        private ResultModel(Status status)
        {
            this._Status = status;
        }
        private ResultModel(string message)
        {
            this._Message = message;
        }
        private ResultModel(Status status, string message)
        {
            this._Status = status;
            this._Message = message;
        }
        private ResultModel(T result, Status status, string message)
        {
            this._Result = result;
            this._Status = status;
            this._Message = message;
        }

        #endregion

        #region property
        private T? _Result { get; set; }
        public T? Result
        {
            get
            {
                return _Result;
            }
        }

        private String? _Message { get; set; }
        public String? Message
        {
            get
            {
                return _Message;
            }
        }

        private Status _Status { get; set; }
        public Status Status
        {
            get
            {
                return _Status;
            }
        }
        #endregion

        #region methods

        public static ResultModel<T> Sucsess()
        {
            return new ResultModel<T>(Status.Success, "عملیات با موفقیت انجام شد");
        }
        public static ResultModel<T> Sucsess(T result)
        {
            return new ResultModel<T>(result, Status.Success, "عملیات با موفقیت انجام شد");
        }
        public static ResultModel<T> Error(string message)
        {
            return new ResultModel<T>(Status.Error, message);
        }
        public static ResultModel<T> ValidationError(string message)
        {
            return new ResultModel<T>(Status.ValidationError, message);
        }
        public static ResultModel<T> NotFound()
        {
            return new ResultModel<T>("آیتم مورد نطر یافت نشد");
        }
        #endregion
    }
}
