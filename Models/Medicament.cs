using System.ComponentModel.DataAnnotations;

namespace MedManager.Models;

public enum MedicamentTypes
{
    [Display(Name = "Pilule")]
    Pill,
    [Display(Name = "Sirop")]
    Syrup,
    [Display(Name = "Crème")]
    Cream,
    [Display(Name = "Gélule")]
    Capsule,
    [Display(Name = "Injection")]
    Injection,
    [Display(Name = "Suppositoire")]
    Suppository,
    [Display(Name = "Pommade")]
    Ointment,
    [Display(Name = "Goutte")]
    Drop,
    [Display(Name = "Inhalateur")]
    Inhaler,
    [Display(Name = "Patch")]
    Patch,
    [Display(Name = "Collyre")]
    EyeDrops,
    [Display(Name = "Autre")]
    Other
}

public enum MedicamentCategories
{
    [Display(Name = "Antidouleur")]
    Analgesic,

    [Display(Name = "Antibiotique")]
    Antibiotic,

    [Display(Name = "Antifongique")]
    Antifungal,

    [Display(Name = "Antiviral")]
    Antiviral,

    [Display(Name = "Antidépresseur")]
    Antidepressant,

    [Display(Name = "Antidiabétique")]
    Antidiabetic,

    [Display(Name = "Antihistaminique")]
    Antihistamine,

    [Display(Name = "Anti-inflammatoire")]
    AntiInflammatory,

    [Display(Name = "Antipsychotique")]
    Antipsychotic,

    [Display(Name = "Anxiolytique")]
    Anxiolytic,

    [Display(Name = "Antiémétique")]
    Antiemetic,

    [Display(Name = "Anticoagulant")]
    Anticoagulant,

    [Display(Name = "Antiplaquettaire")]
    Antiplatelet,

    [Display(Name = "Antispasmodique")]
    Antispasmodic,

    [Display(Name = "Bronchodilatateur")]
    Bronchodilator,

    [Display(Name = "Cardiovasculaire")]
    Cardiovascular,

    [Display(Name = "Corticostéroïde")]
    Corticosteroid,

    [Display(Name = "Dermatologique")]
    Dermatologic,

    [Display(Name = "Diurétique")]
    Diuretic,

    [Display(Name = "Gastro-intestinal")]
    Gastrointestinal,

    [Display(Name = "Hormone")]
    Hormone,

    [Display(Name = "Immunosuppresseur")]
    Immunosuppressant,

    [Display(Name = "Laxatif")]
    Laxative,

    [Display(Name = "Myorelaxant")]
    MuscleRelaxant,

    [Display(Name = "Narcotique")]
    Narcotic,

    [Display(Name = "Neurologique")]
    Neurological,

    [Display(Name = "Ophtalmologique")]
    Ophthalmic,

    [Display(Name = "Ostéoporose")]
    Osteoporosis,

    [Display(Name = "Respiratoire")]
    Respiratory,

    [Display(Name = "Sédatif")]
    Sedative,

    [Display(Name = "Stimulant")]
    Stimulant,

    [Display(Name = "Vaccin")]
    Vaccine,

    [Display(Name = "Supplément vitaminique")]
    VitaminSupplement,

    [Display(Name = "Antinéoplasique")]
    Antineoplastic,

    [Display(Name = "Antiparasitaire")]
    Antiparasitic,

    [Display(Name = "Hypnotique")]
    Hypnotic,

    [Display(Name = "Vasodilatateur")]
    Vasodilator,

    [Display(Name = "Antiépileptique")]
    Antiepileptic,

    [Display(Name = "Probiotique")]
    Probiotic,

    [Display(Name = "Anti-goutte")]
    Antigout,

    [Display(Name = "Urologique")]
    Urological,

    [Display(Name = "Fertilité")]
    Fertility,

    [Display(Name = "Traitement de la migraine")]
    MigraineTreatment,

    [Display(Name = "Antidote")]
    Antidote,

    [Display(Name = "Contraceptif")]
    Contraceptive,

    [Display(Name = "Antithyroïdien")]
    Antithyroid,

    [Display(Name = "Hépatoprotecteur")]
    Hepatoprotective,

    [Display(Name = "Anticholinergique")]
    Anticholinergic,

    [Display(Name = "Ostéoarthrite")]
    Osteoarthritis
}

public class Medicament
{
    [Display(Name = "Médicament Id")]
    public int MedicamentId { get; set; }
    
    [Display(Name = "Date de création")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    [Display(Name = "Nom du médicament")]
    [Required(ErrorMessage = "Le nom du médicament est requis.")]
    [StringLength(256, MinimumLength = 1, ErrorMessage = "Le nom du médicament doit contenir moins de 256 caractères.")]
    public required string Name { get; set; }
    
    [Display(Name = "Quantité du médicament")]
    [Required(ErrorMessage = "La quantité est requise.")]
    [StringLength(1024, MinimumLength = 1, ErrorMessage = "Le quantité du médicament doit contenir moins de 1024 caractères.")]
    public required string Quantity { get; set; }
    
    [Display(Name = "Ingrédients du médicament")]
    [Required(ErrorMessage = "Les ingrédients sont requis.")]
    [StringLength(1024, MinimumLength = 1, ErrorMessage = "Les ingrédients du médicament doivent contenir moins de 1024 caractères.")]
    public required string Ingredients { get; set; }
    
    [Display(Name = "Type de médicament")]
    public required MedicamentTypes Type { get; set; }
    
    [Display(Name = "Catégorie de médicament")]
    public required MedicamentCategories Category { get; set; }


    public List<Prescription> Prescriptions { get; set; } = new();
    public List<Allergy> Allergies { get; set; } = new();
    public List<MedicalHistory> MedicalHistories { get; set; } = new();
}