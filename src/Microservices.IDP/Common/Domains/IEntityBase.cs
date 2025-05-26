namespace Microservices.IDP.Common.Domains;

public interface IEntityBase<T>
{
    T Id { get; set; }
}