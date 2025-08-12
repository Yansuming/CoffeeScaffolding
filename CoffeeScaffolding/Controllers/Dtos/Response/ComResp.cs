namespace CoffeeScaffolding.Controllers.Dtos.Response
{
    public class ComResp
    {
        public ComResp(FlagEnum flag, object data)
        {
            /// <summary>
            /// 标识请求是否成功
            /// </summary>
            this.flag = (int)flag;

            /// <summary>
            /// 返回的数据
            /// </summary>
            this.data = data;
        }
        public int flag;
        public object data;
    }

    public class ListComResp<T>
    {
        public ListComResp(int TotalPages, int CurrentPage, List<T> data)
        {
            this.TotalPages = TotalPages;
            this.CurrentPage = CurrentPage;
            this.data = data;
        }
        public int TotalPages;
        public int CurrentPage;
        public List<T> data;
    }

    public enum FlagEnum
    {
        Fail = 0,// 失败,程序执行时意外错误。
        Success = 1,// 成功
        Waring = 2,// 警告
        Error = 3// 错误，程序执行时主动验证错误。
    }
}