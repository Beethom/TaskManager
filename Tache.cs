class Tache
{
    public int Id { get; set; }
    public string Description { get; set; } = "";
    public bool EstComplete { get; set; }

    public DateTime DateCreation { get; set; } = DateTime.Now;


}