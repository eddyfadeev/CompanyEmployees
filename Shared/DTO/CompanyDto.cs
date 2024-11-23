namespace Shared.DTO;

public record CompanyDto
{
    private readonly Guid _id;
    private readonly string _name;
    private readonly string _address;
    private readonly string _country;

    public CompanyDto(Guid id, string name, string address, string country)
    {
        _id = id;
        _name = name;
        _address = address;
        _country = country;
    }
    
    public Guid Id => _id;
    public string Name => _name;
    public string FullAddress => string.Join(' ', _address, _country);

    public void Deconstruct(out Guid id, out string name, out string address, out string country) =>
        (id, name, address, country) = (_id, _name, _address, _country);
}