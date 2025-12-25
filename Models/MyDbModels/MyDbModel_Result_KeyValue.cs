namespace MyDbModels;

public interface IMyDbModel_Result_KeyValue<TKey,TValue> 
{
    TKey Key { get; set; }
    TValue Value { get; set; }
}

public class MyDbModel_Result_KeyValue<TKey, TValue> : IMyDbModel_Result_KeyValue<TKey, TValue>  
{
    public TKey Key { get; set; }
    public TValue Value { get; set; }
}