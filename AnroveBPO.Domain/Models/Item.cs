using AnroveBPO.Domain.Models.ValueObjects;
using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;

namespace AnroveBPO.Domain.Models;

public sealed class Item : Entity<Guid>
{
    private Item(Guid id,
        string name,
        decimal price,
        string category,
        ItemCode code) : base(id)
    {
        Name = name;
        Price = price;
        Category = category;
        Code = code;
    }
    
    public string Name { get; private set; }
    
    public decimal Price { get; private set; }
    
    public string Category { get; private set; }
    
    public ItemCode Code { get; private set; }

    public UnitResult<Error> UpdateName(string newName)
    {
        //todo тут сделать проверку
        Name = newName;
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> UpdateCode(ItemCode newCode)
    {
        Code = newCode;
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> UpdateCategory(string newCategory)
    {
        Category = newCategory;
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> UpdatePrice(decimal newPrice)
    {
        Price = newPrice;
        return UnitResult.Success<Error>();
    }

    public static Result<Item, Error> Create(
        string name,
        decimal price,
        string category,
        ItemCode code)
    {
        var id = Guid.NewGuid();
        
        //todo тут сделать проверку цены, имени и категории(пока без vo)
        
        return new Item(id, name, price, category, code);
    }
}
