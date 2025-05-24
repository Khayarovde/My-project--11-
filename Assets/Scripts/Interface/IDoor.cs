public interface IDoor
{
    bool IsOpen { get; }
    IRoom ConnectedRoom { get; }  // Изменение на IRoom для ссылки на комнату
    void Open();
    void Close();
}
