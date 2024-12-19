using CargoHub.Models;
using Moq;
namespace CargoHub.Test;
public static class TestHelper
{
    public static bool HaveSameDates(DateTime d1, DateTime d2, TimeSpan tolerance)
    {
        return d1 - d2 <= tolerance;

    }
    public static ItemGroup TestItemGroup1 = new ItemGroup
    {
        Name = "Test Name",
        Description = "Test Description"
    };
    public static ItemGroup TestItemGroup2 = new ItemGroup
    {
        Name = "Updated Name",
        Description = "Updated Description"

    };
    public static ItemGroup InvalidItemGroup = new ItemGroup
    {
        Name = null,
        Description = null
    };
    public static ItemGroup InvalidName_IG = new ItemGroup
    {
        Name = null,
        Description = "Valid Description"
    };


    public static ItemLine TestITemLine1 = new ItemLine
    {
        Name = "Test Name",
        Description = "Test Description"
    };
    public static ItemLine TestItemLine2 = new ItemLine
    {
        Name = "Updated Name",
        Description = "Updated Description"

    };
    public static ItemLine InvalidItemLine = new ItemLine
    {
        Name = null,
        Description = null
    };
    public static ItemLine InvalidName_IL = new ItemLine
    {
        Name = null,
        Description = "Valid Description"
    };
    public static ItemLine InvalidDescription_IL = new ItemLine
    {
        Name = "Valid Name",
        Description = null
    };


}