namespace FinancialAccounts.Web;

using System.ComponentModel.DataAnnotations.Schema;

using Newtonsoft.Json;

/// <summary>
/// ������ �������.
/// </summary>
public class Account
{
    /// <summary>
    /// ���� ��������.
    /// </summary>
    [JsonProperty(PropertyName = nameof(BirthDate))]
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// �������.
    /// </summary>
    [JsonProperty(PropertyName = nameof(Family))]
    public string Family { get; set; }

    /// <summary>
    /// ���������� �������������.
    /// </summary>
    [JsonProperty(PropertyName = nameof(Id))]
    public int Id { get; set; }

    /// <summary>
    /// ���.
    /// </summary>
    [JsonProperty(PropertyName = nameof(Name))]
    public string Name { get; set; }

    /// <summary>
    /// ��������.
    /// </summary>
    [JsonProperty(PropertyName = nameof(SecondName))]
    public string SecondName { get; set; }

    /// <summary>
    /// ������.
    /// </summary>
    [Column(TypeName = "decimal(6, 2)")]
    [JsonProperty(PropertyName = nameof(Sum))]
    public decimal Sum { get; set; }
}