using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domains;
using BL.DTOConfiguration.Base;

namespace BL.DTOConfiguration;

public partial class DTOShipment : BaseDTO, IValidatableObject
{
    [Required(ErrorMessage = "Shipping date is required")]
    [Display(Name = "Shipping Date")]
    public DateTime ShipingDate { get; set; }

    [Required(ErrorMessage = "Delivery date is required")]
    [Display(Name = "Delivery Date")]
    public DateTime DelivryDate { get; set; }

    [Required(ErrorMessage = "Sender is required")]
    [Display(Name = "Sender")]
    public Guid SenderId { get; set; }
    
    public DTOUserSender UserSender { get; set; }

    [Required(ErrorMessage = "Receiver is required")]
    [Display(Name = "Receiver")]
    public Guid ReceiverId { get; set; }
    
    public DTOUserReceiver UserReceiver { get; set; }

    [Required(ErrorMessage = "Shipping type is required")]
    [Display(Name = "Shipping Type")]
    public Guid ShippingTypeId { get; set; }

    [Display(Name = "Shipping Packaging")]
    public Guid? ShipingPackgingId { get; set; }

    [Required(ErrorMessage = "Width is required")]
    [Range(0.1, 1000.0, ErrorMessage = "Width must be between 0.1 and 1000 cm")]
    [Display(Name = "Width (cm)")]
    public double Width { get; set; }

    [Required(ErrorMessage = "Height is required")]
    [Range(0.1, 1000.0, ErrorMessage = "Height must be between 0.1 and 1000 cm")]
    [Display(Name = "Height (cm)")]
    public double Height { get; set; }

    [Required(ErrorMessage = "Weight is required")]
    [Range(0.1, 50000.0, ErrorMessage = "Weight must be between 0.1 and 50000 kg")]
    [Display(Name = "Weight (kg)")]
    public double Weight { get; set; }

    [Required(ErrorMessage = "Length is required")]
    [Range(0.1, 1000.0, ErrorMessage = "Length must be between 0.1 and 1000 cm")]
    [Display(Name = "Length (cm)")]
    public double Length { get; set; }

    [Required(ErrorMessage = "Package value is required")]
    [Range(0.01, 1000000.00, ErrorMessage = "Package value must be between 0.01 and 1,000,000")]
    [Display(Name = "Package Value")]
    public decimal PackageValue { get; set; }

    [Required(ErrorMessage = "Shipping rate is required")]
    [Range(0.01, 10000.00, ErrorMessage = "Shipping rate must be between 0.01 and 10,000")]
    [Display(Name = "Shipping Rate")]
    public decimal ShippingRate { get; set; }

    [Display(Name = "Payment Method")]
    public Guid? PaymentMethodId { get; set; }

    [Display(Name = "User Subscription")]
    public Guid? UserSubscriptionId { get; set; }

    [Range(1000000000, 9999999999999999, ErrorMessage = "Tracking number must be between 10 and 16 digits")]
    [Display(Name = "Tracking Number")]
    public double? TrackingNumber { get; set; }

    [Display(Name = "Reference")]
    public Guid? ReferenceId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        // Validate that delivery date is after shipping date
        if (DelivryDate <= ShipingDate)
        {
            results.Add(new ValidationResult(
                "Delivery date must be after shipping date",
                new[] { nameof(DelivryDate) }));
        }

        // Validate that shipping date is not in the past (allow today)
        if (ShipingDate.Date < DateTime.Today)
        {
            results.Add(new ValidationResult(
                "Shipping date cannot be in the past",
                new[] { nameof(ShipingDate) }));
        }

        // Validate that sender and receiver are different
        if (SenderId == ReceiverId)
        {
            results.Add(new ValidationResult(
                "Sender and receiver cannot be the same",
                new[] { nameof(ReceiverId) }));
        }

        // Validate package dimensions make sense (volume calculation)
        var volume = Width * Height * Length;
        if (volume > 1000000) // 1 cubic meter in cm³
        {
            results.Add(new ValidationResult(
                "Package dimensions are too large. Maximum volume is 1 cubic meter",
                new[] { nameof(Width), nameof(Height), nameof(Length) }));
        }

        // Validate weight vs dimensions ratio (basic density check)
        var volumeInM3 = volume / 1000000; // Convert cm³ to m³
        if (volumeInM3 > 0 && (Weight / volumeInM3) > 10000) // Density > 10,000 kg/m³ (very heavy)
        {
            results.Add(new ValidationResult(
                "Weight seems too high for the given dimensions",
                new[] { nameof(Weight) }));
        }

        return results;
    }
}
