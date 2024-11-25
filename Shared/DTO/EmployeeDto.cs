namespace Shared.DTO;

[Serializable]
public record EmployeeDto
{
    private readonly Guid id;
    private readonly string name;
    private readonly int age;
    private readonly string position;
    
    public EmployeeDto(Guid id, string name, int age, string position)
    {
        this.id = id;
        this.name = name;
        this.age = age;
        this.position = position;
    }

    public Guid Id => id;
    public string Name => name;
    public int Age => age;
    public string Position => position;

    public void Deconstruct(out Guid id, out string name, out int age, out string position) =>
        (id, name, age, position) = (this.id, this.name, this.age, this.position);
}