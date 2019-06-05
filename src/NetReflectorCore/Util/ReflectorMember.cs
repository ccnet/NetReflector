using System;
using System.Reflection;

namespace NetReflectorCore.Util
{
	public abstract class ReflectorMember
	{
		public string Name
		{
			get { return MemberInfo.Name; }
		}

		public string MemberName
		{
			get { return String.Concat(MemberInfo.DeclaringType, ".", MemberInfo.Name); }
		}

		public abstract Type MemberType { get; }

		protected abstract MemberInfo MemberInfo { get; }

		public abstract void SetValue(object instance, object value);
		public abstract object GetValue(object instance);

		protected object ConvertToMemberType(object value)
		{
			return new ReflectorTypeConverter().Convert(MemberType, value);
		}

		public static ReflectorMember Create(MemberInfo memberInfo)
		{
			switch (memberInfo.MemberType)
			{
				case MemberTypes.Property:
					return new ReflectorProperty((PropertyInfo) memberInfo);
				case MemberTypes.Field:
					return new ReflectorField((FieldInfo) memberInfo);
				default:
					throw new NotImplementedException("Only Property and Field member types are currently supported");
			}
		}

		#region ReflectorMember subclasses -- for unifying Member/Field/Property hierarchy

		private class ReflectorProperty : ReflectorMember
		{
			private PropertyInfo info;

			public ReflectorProperty(PropertyInfo propertyInfo) : base()
			{
				info = propertyInfo;
			}

			protected override MemberInfo MemberInfo
			{
				get { return info; }
			}

			public override Type MemberType
			{
				get { return info.PropertyType; }
			}

			public override void SetValue(object instance, object value)
			{
				try
				{
					info.SetValue(instance, ConvertToMemberType(value), null);
				}
				catch (TargetInvocationException ex)
				{
					string msg = string.Format(@"Unable to assign value ""{0}"" to member ""{1}"".", value, MemberName);
					throw new NetReflectorException(msg, ex.InnerException);
				}
				catch (ArgumentException)
				{
					string msg = string.Format(@"Unable to assign value ""{0}"" to member ""{1}"". No set method exists.", value, MemberName);
					throw new NetReflectorException(msg);
				}
			}

			public override object GetValue(object instance)
			{
				return info.GetValue(instance, null);
			}
		}

		private class ReflectorField : ReflectorMember
		{
			private FieldInfo info;

			public ReflectorField(FieldInfo fieldInfo) : base()
			{
				info = fieldInfo;
			}

			protected override MemberInfo MemberInfo
			{
				get { return info; }
			}

			public override Type MemberType
			{
				get { return info.FieldType; }
			}

			public override void SetValue(object instance, object value)
			{
				try
				{
					info.SetValue(instance, ConvertToMemberType(value));
				}
				catch (TargetInvocationException ex)
				{
					string msg = string.Format(@"Unable to assign value ""{0}"" to field ""{1}"".", value, MemberName);
					throw new NetReflectorException(msg, ex.InnerException);
				}
			}

			public override object GetValue(object instance)
			{
				return info.GetValue(instance);
			}
		}

		#endregion
	}
}