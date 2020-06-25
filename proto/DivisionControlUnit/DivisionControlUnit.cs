// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: DivisionControlUnit.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace GrpcDivisionControlUnit {

  /// <summary>Holder for reflection information generated from DivisionControlUnit.proto</summary>
  public static partial class DivisionControlUnitReflection {

    #region Descriptor
    /// <summary>File descriptor for DivisionControlUnit.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static DivisionControlUnitReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChlEaXZpc2lvbkNvbnRyb2xVbml0LnByb3RvEhdHcnBjRGl2aXNpb25Db250",
            "cm9sVW5pdCJkChxSZWdpc3RlckFydGlsbGVyeVVuaXRSZXF1ZXN0Eg8KB3Vu",
            "aXRfaWQYASABKAkSMwoIcG9zaXRpb24YAiABKAsyIS5HcnBjRGl2aXNpb25D",
            "b250cm9sVW5pdC5Qb3NpdGlvbiJnChFSZVBvc2l0aW9uQ29tbWFuZBIzCghw",
            "b3NpdGlvbhgBIAEoCzIhLkdycGNEaXZpc2lvbkNvbnRyb2xVbml0LlBvc2l0",
            "aW9uEh0KFW1haW5fZmlyaW5nX2RpcmVjdGlvbhgCIAEoASJWCgVNZXRlbxIT",
            "Cgt0ZW1wZXJhdHVyZRgBIAEoARISCgp3aW5kX2FuZ2xlGAIgASgBEhIKCndp",
            "bmRfc3BlZWQYAyABKAESEAoIaHVtaWRpdHkYBCABKAEiLwoIUG9zaXRpb24S",
            "EQoJbG9uZ2l0dWRlGAEgASgBEhAKCGxhdGl0dWRlGAIgASgBImIKDkFzc2F1",
            "bHRDb21tYW5kEjMKCHBvc2l0aW9uGAEgASgLMiEuR3JwY0RpdmlzaW9uQ29u",
            "dHJvbFVuaXQuUG9zaXRpb24SGwoTZGlyZWN0aW9uX2RldmlhdGlvbhgCIAEo",
            "ATKxAgoTRGl2aXNpb25Db250cm9sVW5pdBJxCgxSZWdpc3RlclVuaXQSNS5H",
            "cnBjRGl2aXNpb25Db250cm9sVW5pdC5SZWdpc3RlckFydGlsbGVyeVVuaXRS",
            "ZXF1ZXN0GiouR3JwY0RpdmlzaW9uQ29udHJvbFVuaXQuUmVQb3NpdGlvbkNv",
            "bW1hbmQSTQoIR2V0TWV0ZW8SIS5HcnBjRGl2aXNpb25Db250cm9sVW5pdC5Q",
            "b3NpdGlvbhoeLkdycGNEaXZpc2lvbkNvbnRyb2xVbml0Lk1ldGVvElgKCklu",
            "UG9zaXRpb24SIS5HcnBjRGl2aXNpb25Db250cm9sVW5pdC5Qb3NpdGlvbhon",
            "LkdycGNEaXZpc2lvbkNvbnRyb2xVbml0LkFzc2F1bHRDb21tYW5kQhqqAhdH",
            "cnBjRGl2aXNpb25Db250cm9sVW5pdGIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::GrpcDivisionControlUnit.RegisterArtilleryUnitRequest), global::GrpcDivisionControlUnit.RegisterArtilleryUnitRequest.Parser, new[]{ "UnitId", "Position" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::GrpcDivisionControlUnit.RePositionCommand), global::GrpcDivisionControlUnit.RePositionCommand.Parser, new[]{ "Position", "MainFiringDirection" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::GrpcDivisionControlUnit.Meteo), global::GrpcDivisionControlUnit.Meteo.Parser, new[]{ "Temperature", "WindAngle", "WindSpeed", "Humidity" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::GrpcDivisionControlUnit.Position), global::GrpcDivisionControlUnit.Position.Parser, new[]{ "Longitude", "Latitude" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::GrpcDivisionControlUnit.AssaultCommand), global::GrpcDivisionControlUnit.AssaultCommand.Parser, new[]{ "Position", "DirectionDeviation" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class RegisterArtilleryUnitRequest : pb::IMessage<RegisterArtilleryUnitRequest> {
    private static readonly pb::MessageParser<RegisterArtilleryUnitRequest> _parser = new pb::MessageParser<RegisterArtilleryUnitRequest>(() => new RegisterArtilleryUnitRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<RegisterArtilleryUnitRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::GrpcDivisionControlUnit.DivisionControlUnitReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RegisterArtilleryUnitRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RegisterArtilleryUnitRequest(RegisterArtilleryUnitRequest other) : this() {
      unitId_ = other.unitId_;
      position_ = other.position_ != null ? other.position_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RegisterArtilleryUnitRequest Clone() {
      return new RegisterArtilleryUnitRequest(this);
    }

    /// <summary>Field number for the "unit_id" field.</summary>
    public const int UnitIdFieldNumber = 1;
    private string unitId_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string UnitId {
      get { return unitId_; }
      set {
        unitId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "position" field.</summary>
    public const int PositionFieldNumber = 2;
    private global::GrpcDivisionControlUnit.Position position_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::GrpcDivisionControlUnit.Position Position {
      get { return position_; }
      set {
        position_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as RegisterArtilleryUnitRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(RegisterArtilleryUnitRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UnitId != other.UnitId) return false;
      if (!object.Equals(Position, other.Position)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UnitId.Length != 0) hash ^= UnitId.GetHashCode();
      if (position_ != null) hash ^= Position.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (UnitId.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(UnitId);
      }
      if (position_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(Position);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (UnitId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(UnitId);
      }
      if (position_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Position);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(RegisterArtilleryUnitRequest other) {
      if (other == null) {
        return;
      }
      if (other.UnitId.Length != 0) {
        UnitId = other.UnitId;
      }
      if (other.position_ != null) {
        if (position_ == null) {
          Position = new global::GrpcDivisionControlUnit.Position();
        }
        Position.MergeFrom(other.Position);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            UnitId = input.ReadString();
            break;
          }
          case 18: {
            if (position_ == null) {
              Position = new global::GrpcDivisionControlUnit.Position();
            }
            input.ReadMessage(Position);
            break;
          }
        }
      }
    }

  }

  public sealed partial class RePositionCommand : pb::IMessage<RePositionCommand> {
    private static readonly pb::MessageParser<RePositionCommand> _parser = new pb::MessageParser<RePositionCommand>(() => new RePositionCommand());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<RePositionCommand> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::GrpcDivisionControlUnit.DivisionControlUnitReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RePositionCommand() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RePositionCommand(RePositionCommand other) : this() {
      position_ = other.position_ != null ? other.position_.Clone() : null;
      mainFiringDirection_ = other.mainFiringDirection_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RePositionCommand Clone() {
      return new RePositionCommand(this);
    }

    /// <summary>Field number for the "position" field.</summary>
    public const int PositionFieldNumber = 1;
    private global::GrpcDivisionControlUnit.Position position_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::GrpcDivisionControlUnit.Position Position {
      get { return position_; }
      set {
        position_ = value;
      }
    }

    /// <summary>Field number for the "main_firing_direction" field.</summary>
    public const int MainFiringDirectionFieldNumber = 2;
    private double mainFiringDirection_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public double MainFiringDirection {
      get { return mainFiringDirection_; }
      set {
        mainFiringDirection_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as RePositionCommand);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(RePositionCommand other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Position, other.Position)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(MainFiringDirection, other.MainFiringDirection)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (position_ != null) hash ^= Position.GetHashCode();
      if (MainFiringDirection != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(MainFiringDirection);
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (position_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Position);
      }
      if (MainFiringDirection != 0D) {
        output.WriteRawTag(17);
        output.WriteDouble(MainFiringDirection);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (position_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Position);
      }
      if (MainFiringDirection != 0D) {
        size += 1 + 8;
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(RePositionCommand other) {
      if (other == null) {
        return;
      }
      if (other.position_ != null) {
        if (position_ == null) {
          Position = new global::GrpcDivisionControlUnit.Position();
        }
        Position.MergeFrom(other.Position);
      }
      if (other.MainFiringDirection != 0D) {
        MainFiringDirection = other.MainFiringDirection;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            if (position_ == null) {
              Position = new global::GrpcDivisionControlUnit.Position();
            }
            input.ReadMessage(Position);
            break;
          }
          case 17: {
            MainFiringDirection = input.ReadDouble();
            break;
          }
        }
      }
    }

  }

  public sealed partial class Meteo : pb::IMessage<Meteo> {
    private static readonly pb::MessageParser<Meteo> _parser = new pb::MessageParser<Meteo>(() => new Meteo());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Meteo> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::GrpcDivisionControlUnit.DivisionControlUnitReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Meteo() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Meteo(Meteo other) : this() {
      temperature_ = other.temperature_;
      windAngle_ = other.windAngle_;
      windSpeed_ = other.windSpeed_;
      humidity_ = other.humidity_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Meteo Clone() {
      return new Meteo(this);
    }

    /// <summary>Field number for the "temperature" field.</summary>
    public const int TemperatureFieldNumber = 1;
    private double temperature_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public double Temperature {
      get { return temperature_; }
      set {
        temperature_ = value;
      }
    }

    /// <summary>Field number for the "wind_angle" field.</summary>
    public const int WindAngleFieldNumber = 2;
    private double windAngle_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public double WindAngle {
      get { return windAngle_; }
      set {
        windAngle_ = value;
      }
    }

    /// <summary>Field number for the "wind_speed" field.</summary>
    public const int WindSpeedFieldNumber = 3;
    private double windSpeed_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public double WindSpeed {
      get { return windSpeed_; }
      set {
        windSpeed_ = value;
      }
    }

    /// <summary>Field number for the "humidity" field.</summary>
    public const int HumidityFieldNumber = 4;
    private double humidity_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public double Humidity {
      get { return humidity_; }
      set {
        humidity_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Meteo);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Meteo other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Temperature, other.Temperature)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(WindAngle, other.WindAngle)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(WindSpeed, other.WindSpeed)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Humidity, other.Humidity)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Temperature != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Temperature);
      if (WindAngle != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(WindAngle);
      if (WindSpeed != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(WindSpeed);
      if (Humidity != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Humidity);
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Temperature != 0D) {
        output.WriteRawTag(9);
        output.WriteDouble(Temperature);
      }
      if (WindAngle != 0D) {
        output.WriteRawTag(17);
        output.WriteDouble(WindAngle);
      }
      if (WindSpeed != 0D) {
        output.WriteRawTag(25);
        output.WriteDouble(WindSpeed);
      }
      if (Humidity != 0D) {
        output.WriteRawTag(33);
        output.WriteDouble(Humidity);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Temperature != 0D) {
        size += 1 + 8;
      }
      if (WindAngle != 0D) {
        size += 1 + 8;
      }
      if (WindSpeed != 0D) {
        size += 1 + 8;
      }
      if (Humidity != 0D) {
        size += 1 + 8;
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Meteo other) {
      if (other == null) {
        return;
      }
      if (other.Temperature != 0D) {
        Temperature = other.Temperature;
      }
      if (other.WindAngle != 0D) {
        WindAngle = other.WindAngle;
      }
      if (other.WindSpeed != 0D) {
        WindSpeed = other.WindSpeed;
      }
      if (other.Humidity != 0D) {
        Humidity = other.Humidity;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 9: {
            Temperature = input.ReadDouble();
            break;
          }
          case 17: {
            WindAngle = input.ReadDouble();
            break;
          }
          case 25: {
            WindSpeed = input.ReadDouble();
            break;
          }
          case 33: {
            Humidity = input.ReadDouble();
            break;
          }
        }
      }
    }

  }

  public sealed partial class Position : pb::IMessage<Position> {
    private static readonly pb::MessageParser<Position> _parser = new pb::MessageParser<Position>(() => new Position());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Position> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::GrpcDivisionControlUnit.DivisionControlUnitReflection.Descriptor.MessageTypes[3]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Position() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Position(Position other) : this() {
      longitude_ = other.longitude_;
      latitude_ = other.latitude_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Position Clone() {
      return new Position(this);
    }

    /// <summary>Field number for the "longitude" field.</summary>
    public const int LongitudeFieldNumber = 1;
    private double longitude_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public double Longitude {
      get { return longitude_; }
      set {
        longitude_ = value;
      }
    }

    /// <summary>Field number for the "latitude" field.</summary>
    public const int LatitudeFieldNumber = 2;
    private double latitude_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public double Latitude {
      get { return latitude_; }
      set {
        latitude_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Position);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Position other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Longitude, other.Longitude)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Latitude, other.Latitude)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Longitude != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Longitude);
      if (Latitude != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Latitude);
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Longitude != 0D) {
        output.WriteRawTag(9);
        output.WriteDouble(Longitude);
      }
      if (Latitude != 0D) {
        output.WriteRawTag(17);
        output.WriteDouble(Latitude);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Longitude != 0D) {
        size += 1 + 8;
      }
      if (Latitude != 0D) {
        size += 1 + 8;
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Position other) {
      if (other == null) {
        return;
      }
      if (other.Longitude != 0D) {
        Longitude = other.Longitude;
      }
      if (other.Latitude != 0D) {
        Latitude = other.Latitude;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 9: {
            Longitude = input.ReadDouble();
            break;
          }
          case 17: {
            Latitude = input.ReadDouble();
            break;
          }
        }
      }
    }

  }

  public sealed partial class AssaultCommand : pb::IMessage<AssaultCommand> {
    private static readonly pb::MessageParser<AssaultCommand> _parser = new pb::MessageParser<AssaultCommand>(() => new AssaultCommand());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<AssaultCommand> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::GrpcDivisionControlUnit.DivisionControlUnitReflection.Descriptor.MessageTypes[4]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AssaultCommand() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AssaultCommand(AssaultCommand other) : this() {
      position_ = other.position_ != null ? other.position_.Clone() : null;
      directionDeviation_ = other.directionDeviation_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AssaultCommand Clone() {
      return new AssaultCommand(this);
    }

    /// <summary>Field number for the "position" field.</summary>
    public const int PositionFieldNumber = 1;
    private global::GrpcDivisionControlUnit.Position position_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::GrpcDivisionControlUnit.Position Position {
      get { return position_; }
      set {
        position_ = value;
      }
    }

    /// <summary>Field number for the "direction_deviation" field.</summary>
    public const int DirectionDeviationFieldNumber = 2;
    private double directionDeviation_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public double DirectionDeviation {
      get { return directionDeviation_; }
      set {
        directionDeviation_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as AssaultCommand);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(AssaultCommand other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Position, other.Position)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(DirectionDeviation, other.DirectionDeviation)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (position_ != null) hash ^= Position.GetHashCode();
      if (DirectionDeviation != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(DirectionDeviation);
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (position_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Position);
      }
      if (DirectionDeviation != 0D) {
        output.WriteRawTag(17);
        output.WriteDouble(DirectionDeviation);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (position_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Position);
      }
      if (DirectionDeviation != 0D) {
        size += 1 + 8;
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(AssaultCommand other) {
      if (other == null) {
        return;
      }
      if (other.position_ != null) {
        if (position_ == null) {
          Position = new global::GrpcDivisionControlUnit.Position();
        }
        Position.MergeFrom(other.Position);
      }
      if (other.DirectionDeviation != 0D) {
        DirectionDeviation = other.DirectionDeviation;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            if (position_ == null) {
              Position = new global::GrpcDivisionControlUnit.Position();
            }
            input.ReadMessage(Position);
            break;
          }
          case 17: {
            DirectionDeviation = input.ReadDouble();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
