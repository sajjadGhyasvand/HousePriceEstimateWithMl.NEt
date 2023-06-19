using Microsoft.ML.Data;

public class Advertisement
{
    //Area,BuildYear,Rooms,Floor,Elevator,Parking,Storage,LocationName,TotalPrice
    [LoadColumn(0)]
    public float Area;
    [LoadColumn(1)]
    public float BuildYear;
    [LoadColumn(2)]
    public float Rooms;
    [LoadColumn(3)]
    public float Floor;
    [LoadColumn(4)]
    public bool Elevator ;
    [LoadColumn(5)]
    public bool Parking;
    [LoadColumn(6)]
    public bool Storage;
    [LoadColumn(7)]
    public string LocationName;
    [LoadColumn(8)]
    public float TotalPrice;
}