namespace Torman.Swipeable.Managers
{
    public interface ISwipeManager
    {
        void Bind(SwipeLayout view);
        void Unbind(SwipeLayout view);
        void Open(SwipeLayout view);
        void Close(SwipeLayout view);
        void UnbindAll();
        void CloseAll();
    }
}