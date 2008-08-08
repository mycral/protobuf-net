﻿using System;
using ProtoBuf.ProtoBcl;

namespace ProtoBuf.Property
{

    internal sealed class PropertyGuidGroup<TSource> : Property<TSource, Guid>
    {
        public override string DefinedType
        {
            get { return "bcl.Guid"; }
        }
        public override WireType WireType { get { return WireType.StartGroup; } }
        protected override void OnAfterInit()
        {
            base.OnAfterInit();
            suffix = GetPrefix(Tag, WireType.EndGroup);
        }
        private uint suffix;

        public override int Serialize(TSource source, SerializationContext context)
        {
            Guid value = GetValue(source);
            if (IsOptional && value == DefaultValue) return 0;
            return WritePrefix(context)
                + ProtoGuid.Serialize(value, context, false)
                + Base128Variant.EncodeUInt32(suffix, context);
        }

        public override Guid DeserializeImpl(TSource source, SerializationContext context)
        {
            context.StartGroup(Tag); // will be ended internally
            return ProtoGuid.Deserialize(context);
        }
    }
}