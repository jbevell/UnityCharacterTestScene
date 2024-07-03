// Producers only create objects when interacted with.
// Converters primarily accept objects and convert them into new objects.
// Consumers primarily accept objects and do not create any objects in return.
// Some of these types may overlap slightly if a station does more than one. But each station will have a primary function that the type is based on.

public enum StationType
{
	Producer,
	Converter,
	Consumer
}