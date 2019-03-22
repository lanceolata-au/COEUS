using System;
using System.ComponentModel.DataAnnotations;
using carbon.core.Interfaces;

namespace carbon.core.Features
{
    public class Entity<TId> : IEntity<TId> where TId: struct
    {
        [Key]
        public virtual TId Id { get; protected set; }

        protected Entity(){}
        
        protected Entity(TId id)
        {
            if (object.Equals(id,default(TId)))
            {
                throw new ArgumentException("The identifier cannot be default.", paramName: nameof(id));
            }

            // ReSharper disable once VirtualMemberCallInConstructor
            this.Id = id;
        }

        public override bool Equals(object otherObject)
        {
            if (otherObject is Entity<TId> entity && !Equals(Id, default(TId)))
            {
                return this.Equals(entity);
            }
            return base.Equals(otherObject);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        private bool Equals(Entity<TId> other)
        {
            return other != null && this.Id.Equals(other.Id);
        }
        
    }
    
}