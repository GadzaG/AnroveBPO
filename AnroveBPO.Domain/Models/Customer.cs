using AnroveBPO.Domain.Models.ValueObjects;
using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;

namespace AnroveBPO.Domain.Models;

public sealed class Customer : Entity<Guid>
{
    private Customer()
    {
        
    }

    private Customer(
        Guid id,
        string name,
        CustomerCode code,
        Address address,
        Discount? discount) 
        : base(id)
    {
        Name = name;
        Code = code;
        Address = address;
        Discount = discount;
    }
    
    public string Name { get; private set; }
    
    public CustomerCode Code { get; private set; }
    
    public Address Address { get; private set; }
    
    public Discount? Discount { get; private set; }
    
    public static Result<Customer, Error> Create(string name,  CustomerCode code, Address address, Discount? discount = null)
    {
        var id = Guid.NewGuid();
        return new Customer(id, name, code, address, discount);
    }
}