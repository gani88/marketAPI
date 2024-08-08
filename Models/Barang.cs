public class Barang
{
    public int Id { get; set; }
    public string KodeBarang { get; set; }
    public string NamaBarang { get; set; }
    public decimal HargaBarang { get; set; }
    public int JumlahBarang { get; set; }
    private DateTime _expiredDate;
    public DateTime ExpiredDate
    {
        get => _expiredDate;
        set => _expiredDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
    public int GudangId { get; set; }
    public Gudang? Gudang { get; set; }
}

/**
Id: An integer representing the unique identifier of the product.
KodeBarang: A string representing the code of the product.
NamaBarang: A string representing the name of the product.
HargaBarang: A decimal representing the price of the product.
JumlahBarang: An integer representing the quantity of the product in stock.
ExpiredDate: A DateTime representing the expiration date of the product.
GudangId: An integer representing the ID of the warehouse where the product is stored.
Gudang: A reference to a Gudang object representing the warehouse where the product is stored.
**/