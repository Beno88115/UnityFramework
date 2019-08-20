using SimpleJSON;

public class ConfigGiftRow : ConfigRow
{
    public int GiftId { get; private set; }
	public int Name { get; private set; }
	public int Type { get; private set; }
	public int Price2 { get; private set; }
	public int Des { get; private set; }
	public string Related { get; private set; }

	protected override void Parse(JSONNode node)
	{
		GiftId = node[1].AsInt;
		Name = node[2].AsInt;
		Type = node[3].AsInt;
		Price2 = node[4].AsInt;
		Des = node[5].AsInt;
		Related = node[6];
	}
}

public class ConfigPropRow : ConfigRow
{
	public int Name { get; private set; }
	public int Desc { get; private set; }
	public int Value { get; private set; }
	public int Limit { get; private set; }
	public int StoreSwitch { get; private set; }
	public int Cost { get; private set; }
	public int Money { get; private set; }
	public string Tex { get; private set; }

	protected override void Parse(JSONNode node)
	{
		Name = node[1].AsInt;
		Desc = node[2].AsInt;
		Value = node[3].AsInt;
		Limit = node[4].AsInt;
		StoreSwitch = node[5].AsInt;
		Cost = node[6].AsInt;
		Money = node[7].AsInt;
		Tex = node[8];
	}
}

public class ConfigShopRow : ConfigRow
{
	public string Key { get; private set; }
	public int CurrencyType { get; private set; }
	public float Price { get; private set; }
	public int GiveItem { get; private set; }
	public int Num { get; private set; }
	public string Picture { get; private set; }
	public int Desc { get; private set; }

	protected override void Parse(JSONNode node)
	{
		Key = node[1];
		CurrencyType = node[2].AsInt;
		Price = node[3].AsFloat;
		GiveItem = node[4].AsInt;
		Num = node[5].AsInt;
		Picture = node[6];
		Desc = node[7].AsInt;
	}
}