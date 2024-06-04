using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class Bouncy : RuleTile<Bouncy.Neighbor>
{
	public bool customField;

	public class Neighbor : RuleTile.TilingRule.Neighbor
	{
		public const int Null = 3;
		public const int NotNull = 4;
	}

	public override bool RuleMatch(int neighbor, TileBase tile)
	{
		switch (neighbor)
		{
			case Neighbor.Null: return tile == null;
			case Neighbor.NotNull: return tile != null;
		}
		return base.RuleMatch(neighbor, tile);
	}
}