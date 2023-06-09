namespace backgroundJob.Database.Entity
{
	public class BaseEntity
	{
		public Guid Id { get; set; }

		public Guid? CreatedBy { get; set; }
		public DateTime? CreatedOn { get; set; }

		public Guid? ModifiedBy { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public string? ModifiedReason { get; set; }

		public bool IsDeleted { get; set; }
		public Guid? DeletedBy { get; set; }
		public DateTime? DeletedOn { get; set; }
		public string? DeletedReason { get; set; }
	}
}
