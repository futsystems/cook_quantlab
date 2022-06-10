
using System.ComponentModel.DataAnnotations;

namespace UniCryptoLab.Web.Framework
{
    /// <summary>
    /// Port 	6191
    /// </summary>
    public enum Permissions : ushort
    {
        //10
        [Display(GroupName = "商品", Name = "查看", Description = "查看商品")]
        Product = 2900,
        [Display(GroupName = "商品", Name = "创建", Description = "创建商品")]
        ProductCreate = 2901,
        [Display(GroupName = "商品", Name = "删除", Description = "删除商品")]
        ProductDelete = 2902,
        [Display(GroupName = "商品", Name = "更新", Description = "更新商品")]
        ProductUpdate = 2903,
        [Display(GroupName = "商品", Name = "审核", Description = "审核商品")]
        ProductAudit = 2904,
        [Display(GroupName = "商品", Name = "更新库存", Description = "更新商品库存")]
        ProductStockUpdate = 2905,
        [Display(GroupName = "商品", Name = "上架", Description = "上架商品")]
        ProductOnSale = 2906,


        //5
        [Display(GroupName = "商品类目", Name = "查看", Description = "查看商品类目")]
        ProductCategory = 2910,
        [Display(GroupName = "商品类目", Name = "创建", Description = "创建商品类目")]
        ProductCategoryCreate = 2911,
        [Display(GroupName = "商品类目", Name = "删除", Description = "删除商品类目")]
        ProductCategoryDelete = 2912,
        [Display(GroupName = "商品类目", Name = "更新", Description = "更新商品类目")]
        ProductCategoryUpdate = 2913,


        //5
        [Display(GroupName = "商品分组", Name = "查看", Description = "查看商品分组")]
        ProductTag = 2916,
        [Display(GroupName = "商品分组", Name = "创建", Description = "创建商品分组")]
        ProductTagCreate = 2917,
        [Display(GroupName = "商品分组", Name = "删除", Description = "删除商品分组")]
        ProductTagDelete = 2918,
        [Display(GroupName = "商品分组", Name = "更新", Description = "更新商品分组")]
        ProductTagUpdate = 2919,

    }
}
