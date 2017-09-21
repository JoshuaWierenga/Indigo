using Indigo.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Indigo.Server.Context
{
    public class IndigoContext : DbContext
    {
		public IndigoContext(DbContextOptions<IndigoContext> options) : base(options)
		{
		}

		public DbSet<Conversation> Conversations { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<UserConversation> UserConversations { get; set; }
		public DbSet<Message> Messages { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserConversation>()
				.HasKey(t => new { t.UserId, t.ConversationId });

			modelBuilder.Entity<UserConversation>()
			 .HasOne(uc => uc.User)
			 .WithMany(u => u.UserConversations)
			 .HasForeignKey(uc => uc.UserId);

			modelBuilder.Entity<UserConversation>()
			 .HasOne(uc => uc.Conversation)
			 .WithMany(c => c.UserConversations)
			 .HasForeignKey(uc => uc.ConversationId);
		}
	}
}