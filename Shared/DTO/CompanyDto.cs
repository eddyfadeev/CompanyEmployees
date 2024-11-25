namespace Shared.DTO;

[Serializable]
public record CompanyDto
{
    private readonly Guid id;
    private readonly string name;
    private readonly string address;
    private readonly string country;

    public CompanyDto(Guid id, string name, string address, string country)
    {
        this.id = id;
        this.name = name;
        this.address = address;
        this.country = country;
    }
    
    public Guid Id => id;
    public string Name => name;
    public string FullAddress => string.Join(' ', address, country);

    public void Deconstruct(out Guid id, out string name, out string address, out string country) =>
        (id, name, address, country) = (this.id, this.name, this.address, this.country);
}