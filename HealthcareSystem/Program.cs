namespace HealthcareSystem
{
    // Generic Repository
    public class Repository<T>
    {
        private List<T> items = new List<T>();

        public void Add(T item)
        {
            items.Add(item);
        }

        public List<T> GetAll()
        {
            return new List<T>(items);
        }

        public T? GetById(Func<T, bool> predicate)
        {
            return items.FirstOrDefault(predicate);
        }

        public bool Remove(Func<T, bool> predicate)
        {
            var item = items.FirstOrDefault(predicate);
            if (item != null)
            {
                items.Remove(item);
                return true;
            }
            return false;
        }
    }

    // Patient Entity
    public class Patient
    {
        public int Id { get; }
        public string Name { get; }
        public int Age { get; }
        public string Gender { get; }

        public Patient(int id, string name, int age, string gender)
        {
            Id = id;
            Name = name;
            Age = age;
            Gender = gender;
        }

        public override string ToString()
        {
            return $"Patient ID: {Id}, Name: {Name}, Age: {Age}, Gender: {Gender}";
        }
    }

    // Prescription Entity
    public class Prescription
    {
        public int Id { get; }
        public int PatientId { get; }
        public string MedicationName { get; }
        public DateTime DateIssued { get; }

        public Prescription(int id, int patientId, string medicationName, DateTime dateIssued)
        {
            Id = id;
            PatientId = patientId;
            MedicationName = medicationName;
            DateIssued = dateIssued;
        }

        public override string ToString()
        {
            return $"Prescription ID: {Id}, Patient ID: {PatientId}, Medication: {MedicationName}, Date: {DateIssued.ToShortDateString()}";
        }
    }

    // HealthSystemApp
    public class HealthSystemApp
    {
        private Repository<Patient> _patientRepo = new Repository<Patient>();
        private Repository<Prescription> _prescriptionRepo = new Repository<Prescription>();
        private Dictionary<int, List<Prescription>> _prescriptionMap = new Dictionary<int, List<Prescription>>();

        public void SeedData()
        {
            // Add Patients
            _patientRepo.Add(new Patient(1, "John Doe", 30, "Male"));
            _patientRepo.Add(new Patient(2, "Jane Smith", 25, "Female"));
            _patientRepo.Add(new Patient(3, "Michael Brown", 40, "Male"));

            // Add Prescriptions
            _prescriptionRepo.Add(new Prescription(101, 1, "Paracetamol", DateTime.Now.AddDays(-10)));
            _prescriptionRepo.Add(new Prescription(102, 1, "Ibuprofen", DateTime.Now.AddDays(-5)));
            _prescriptionRepo.Add(new Prescription(103, 2, "Amoxicillin", DateTime.Now.AddDays(-7)));
            _prescriptionRepo.Add(new Prescription(104, 3, "Vitamin C", DateTime.Now.AddDays(-2)));
            _prescriptionRepo.Add(new Prescription(105, 2, "Cetirizine", DateTime.Now.AddDays(-1)));
        }

        public void BuildPrescriptionMap()
        {
            _prescriptionMap.Clear();

            foreach (var prescription in _prescriptionRepo.GetAll())
            {
                if (!_prescriptionMap.ContainsKey(prescription.PatientId))
                {
                    _prescriptionMap[prescription.PatientId] = new List<Prescription>();
                }
                _prescriptionMap[prescription.PatientId].Add(prescription);
            }
        }

        public void PrintAllPatients()
        {
            Console.WriteLine("\n--- All Patients ---");
            foreach (var patient in _patientRepo.GetAll())
            {
                Console.WriteLine(patient);
            }
        }

        public void PrintPrescriptionsForPatient(int patientId)
        {
            if (_prescriptionMap.ContainsKey(patientId))
            {
                Console.WriteLine($"\n--- Prescriptions for Patient ID {patientId} ---");
                foreach (var prescription in _prescriptionMap[patientId])
                {
                    Console.WriteLine(prescription);
                }
            }
            else
            {
                Console.WriteLine($"\nNo prescriptions found for Patient ID {patientId}.");
            }
        }
    }

    // Program Entry Point
    class Program
    {
        static void Main()
        {
            HealthSystemApp app = new HealthSystemApp();

            app.SeedData();
            app.BuildPrescriptionMap();

            app.PrintAllPatients();

            Console.Write("\nEnter Patient ID to view prescriptions: ");
            if (int.TryParse(Console.ReadLine(), out int patientId))
            {
                app.PrintPrescriptionsForPatient(patientId);
            }
            else
            {
                Console.WriteLine("Invalid Patient ID.");
            }
        }
    }
}
