namespace Framework
{
    public interface IUIPanelMsg
    {
        void Process(int msgId, params object[] param);
    }
}