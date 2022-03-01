namespace FinancialAccounts.Web;

using System.ComponentModel.DataAnnotations.Schema;

using Newtonsoft.Json;

/// <summary>
/// Модель клиента.
/// </summary>
public class Account
{
    /// <summary>
    /// Дата рождения.
    /// </summary>
    [JsonProperty(PropertyName = nameof(BirthDate))]
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Фамилия.
    /// </summary>
    [JsonProperty(PropertyName = nameof(Family))]
    public string Family { get; set; }

    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    [JsonProperty(PropertyName = nameof(Id))]
    public int Id { get; set; }

    /// <summary>
    /// Имя.
    /// </summary>
    [JsonProperty(PropertyName = nameof(Name))]
    public string Name { get; set; }

    /// <summary>
    /// Отчество.
    /// </summary>
    [JsonProperty(PropertyName = nameof(SecondName))]
    public string SecondName { get; set; }

    /// <summary>
    /// Баланс.
    /// </summary>
    [Column(TypeName = "decimal(6, 2)")]
    [JsonProperty(PropertyName = nameof(Sum))]
    public decimal Sum { get; set; }
}