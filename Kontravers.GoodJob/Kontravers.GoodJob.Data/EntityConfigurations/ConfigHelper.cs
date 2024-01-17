using System.Linq.Expressions;
using Kontravers.GoodJob.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kontravers.GoodJob.Data.EntityConfigurations;

public static class ConfigHelper
{
    public const string TalentSchema = "Talent";
    public const string WorkSchema = "Work";
    
    public static void SetRequired<TEntityType>(this EntityTypeBuilder<TEntityType> builder,
        params Expression<Func<TEntityType, object>>[] setters) where TEntityType : class, IEntity
    {
        foreach (var setter in setters)
        {
            var member = setter.Body;
            if (member == null)
            {
                throw new ArgumentNullException(nameof(setter.Body));
            }

            var propInfo = member is UnaryExpression expression 
                ? expression.Operand as MemberExpression
                : member as MemberExpression;
            
            builder.Property(propInfo!.Member.Name).IsRequired();
        }
    }
}