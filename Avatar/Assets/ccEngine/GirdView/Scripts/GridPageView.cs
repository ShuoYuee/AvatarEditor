using System.Collections.Generic;
using System.Linq;
using ccU3DEngine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



/// <summary>
/// 格子翻页组件
/// </summary>
public class GridPageView : MonoBehaviour, IEndDragHandler
{
    int _iMaxSelNum = 1;
    /// <summary>
    /// 保存当前选择的项目
    /// </summary>
    private List<GridItem> _aCurSelItem = new List<GridItem>();
    /// <summary>
    /// 格子被点击回调事件
    /// </summary>
    private ccCallback _CallBack_OnClick = null;
    /// <summary>
    /// 是否锁定拉动功能 true锁定不能拉动 false可以拉动
    /// </summary>
    private bool _bLockDrag = false;
    public ScrollRect m_ScrollRect;
    int _iItemWidth;
    int _iItemHeight;

    //item容器
    public GameObject content;
    //滚动组件
    public GameObject scroll;
    //item的预设
    public GameObject itemPrefab = null;
    public int _showPageCount = 2;

    //是否是横向的
    private bool isHorizontal;
    //页数
    private int _pageCount = 0;
    public int pageCount { get { return _pageCount; } }
    //一页中的格子数量
    private int cellsMaxCountInPage;
    //横向间隔
    private float gapH;
    //纵向间隔
    private float gapV;
    ////元素宽度
    //private float itemWidth;
    ////元素高度
    //private float itemHeight;
    //content的transform组件
    private RectTransform contentRectTf;
    private List<GridItemDT> _aAllGridItem = null;
    /// <summary>
    /// 当前处于显示队列里的数据
    /// </summary>
    private List<GridItemDT> _aCurShowGridItem = null;

    /// <summary>
    /// 存放页数的列表
    /// </summary>
    private List<GameObject> pageList = null;
    /// <summary>
    /// 存放每页的item列表
    /// </summary>
    private List<List<GridItem>> pageItemList = null;
    //总格子数
    public int cellCount
    {
        get
        {
            if (_aCurShowGridItem == null) { return 0; }
            return _aCurShowGridItem.Count;
        }
    }
    //行数(决定list的高度)
    private int _iRows;
    //列数(决定list的宽度)
    private int _iColumns;
    //当前页数
    private int curPageIndex;
    public int pageIndex { get { return curPageIndex; } }
    //滚动组件
    private ScrollRect sr;
    //单页的高宽
    private Vector2 pageSize;
    //可显示的页数
    private int showPageCount = 2;
    //底部位置
    private float bottom;
    //顶部位置
    private float top;
    //左边位置
    private float left;
    //右边位置
    private float right;
    //最后一页可显示item的数量
    private int lastPageItemCount;
    //第一页的位置
    private Vector2 firstPagePos;

    private int _iCurClass = -99;
    public List<int> _SubMenuList = new List<int>();
    private List<long> _SubItemList = new List<long>();
    //記錄每個菜單已選取Item
    private Dictionary<int, long> _SelDictionary = new Dictionary<int, long>();

    //保持首頁存在
    public bool _KeepPage = true;
    //保持每頁選擇項目
    public bool _SelKeep = false;

    public void f_Reset()
    {
        if (_aCurShowGridItem != null)
        {
            _aCurShowGridItem.Clear();
        }
        if (_aAllGridItem != null)
        {
            _aAllGridItem.Clear();
        }
        f_ClearCurSelItem();

        _CallBack_OnClick = null;
        curPageIndex = 0;
        _pageCount = 0;
        showPageCount = _showPageCount;
        _SelDictionary.Clear();
        _SubItemList.Clear();
        f_ResetPage();
    }

    public void f_ResetPage()
    {
        if (pageItemList != null)
        {
            pageItemList.Clear();
            pageItemList = null;
        }
        if (pageList != null)
        {
            pageList.Clear();
            pageList = null;
        }
        this._pageCount = 0;
        this.showPageCount = _showPageCount;
        cellsMaxCountInPage = 0;
        ccMathEx.f_CreateChild(content, 1, true);
        itemPrefab.SetActive(true);
    }

    #region Init
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="rows">一页的行数</param>
    /// <param name="columns">一页的列数</param>
    /// <param name="sourceData">格子要显示的数据列表</param>
    /// <param name="CallBack_OnClick">格子被点击回调事件</param>
    /// <param name="isHorizontal">是否横向</param>
    /// <param name="gapH">格子的横向间隔</param>
    /// <param name="gapV">格子的纵向间隔</param>
    /// <param name="bLockDrag">是否锁定拉动功能 true锁定不能拉动 false可以拉动</param>
    public void f_Init(int iMaxSelNum, int iSortClass, List<GridItemDT> aGridItem, int iItemWidth, int iItemHeight, int rows,
                     int columns,
                     ccCallback CallBack_OnClick,
                     bool isHorizontal = true,
                     float gapH = 5,
                     float gapV = 5,
                    bool bLockDrag = false)
    {
        if (aGridItem == null)
        {
            Debug.LogError("数据列表不能为空");
            return;
        }
        _iMaxSelNum = iMaxSelNum;
        _aAllGridItem = aGridItem;
        _CallBack_OnClick = CallBack_OnClick;

        f_Sort(iSortClass, false);

        Init(iItemWidth, iItemHeight, rows, columns, isHorizontal, gapH, gapV, bLockDrag);

    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="rows">一页的行数</param>
    /// <param name="columns">一页的列数</param>
    /// <param name="sourceData">格子要显示的数据列表</param>
    /// <param name="CallBack_OnClick">格子被点击回调事件</param>
    /// <param name="isHorizontal">是否横向</param>
    /// <param name="gapH">格子的横向间隔</param>
    /// <param name="gapV">格子的纵向间隔</param>
    /// <param name="bLockDrag">是否锁定拉动功能 true锁定不能拉动 false可以拉动</param>
    public void f_Init(List<NBaseSCDT> sourceData, int iItemWidth, int iItemHeight, int rows,
                     int columns,
                     ccCallback CallBack_OnClick,
                     bool isHorizontal = true,
                     float gapH = 5,
                     float gapV = 5,
                    bool bLockDrag = false)
    {
        //if (sourceData == null)
        //{
        //    Debug.LogError("数据列表不能为空");
        //    return;
        //}
        //_CallBack_OnClick = CallBack_OnClick;        
        //_cellCount = sourceData.Count;

        //_aGridItem.Clear();
        //for (int i = 0; i < _cellCount; i++)
        //{
        //    GridItemDT tGridItemDT = new GridItemDT(CallBack_OnItemClick);
        //    tGridItemDT.f_InitData("iIndex:" + i, sourceData[i]);
        //    _aGridItem.Add(tGridItemDT);
        //}

        //Init(iItemWidth, iItemHeight, rows, columns, isHorizontal, gapH, gapV, bLockDrag);
    }

    private void Init(int iItemWidth, int iItemHeight,
                     int rows, int columns,
                     bool isHorizontal,
                     float gapH, float gapV,
                    bool bLockDrag)
    {
        _bLockDrag = bLockDrag;
        _iItemWidth = iItemWidth;
        _iItemHeight = iItemHeight;
        _iRows = rows;
        _iColumns = columns;

        //一页多少格子
        this.cellsMaxCountInPage = rows * columns;

        if (this.scroll == null) return;
        if (this.content == null) return;

        //this._cellCount = cellCount;
        this.isHorizontal = isHorizontal;
        this.gapH = gapH;
        this.gapV = gapV;

        this.curPageIndex = 0;

        //this.itemWidth = this.itemPrefab.GetComponent<RectTransform>().sizeDelta.x;
        //this.itemHeight = this.itemPrefab.GetComponent<RectTransform>().sizeDelta.y;

        m_ScrollRect = scroll.GetComponent<ScrollRect>();
        m_ScrollRect.onValueChanged.AddListener(OnMove);
        m_ScrollRect.enabled = _bLockDrag ? false : true;

        //计算单页高宽
        this.pageSize = new Vector2(this._iColumns * (this._iItemWidth + this.gapH),
                                    this._iRows * (this._iItemHeight + this.gapV));

        MessageBox.DEBUG("pageSize " + pageSize.ToString());
        //计算显示面板，取消此处计算改由UI显示控制
        this.scroll.GetComponent<RectTransform>().sizeDelta = pageSize;
        this.sr = this.scroll.GetComponent<ScrollRect>();
        this.sr.horizontal = this.isHorizontal;
        this.sr.vertical = !this.isHorizontal;
        this.contentRectTf = this.content.GetComponent<RectTransform>();
        this.content.transform.localPosition = new Vector3(0, 0);
        this.firstPagePos = new Vector2();

        this.CreateData();

        this.updatePage();
        //重新调用item回调
        this.UpdateItem();

        rollPosByPageByIndex(0);

        this.itemPrefab.SetActive(false);

    }
    #endregion

    private void OnMove(Vector2 T)
    {
        this.updatePage();

    }

    public void f_Sort(int iClass, bool bUpdate)
    {
        _iCurClass = iClass;
        if (iClass == -99)
        {
            //_aCurShowGridItem = _aAllGridItem;
            List<GridItemDT> aList = new List<GridItemDT>();
            List<long> iItem = new List<long>();
            for (int i = 0; i < _aAllGridItem.Count; i++)
            {
                if (!_SubMenuList.Contains(_aAllGridItem[i].f_GetClass()) && !_SubItemList.Contains(_aAllGridItem[i].m_iId) && !iItem.Contains(_aAllGridItem[i].m_iId))
                {
                    aList.Add(_aAllGridItem[i]);
                    iItem.Add(_aAllGridItem[i].m_iId);
                }
            }
            _aCurShowGridItem = aList;
        }
        else
        {
            _aCurShowGridItem = _aAllGridItem.Where(delegate (GridItemDT tItem)
            {
                if (tItem.f_GetClass() == iClass)
                {
                    return true;
                }
                return false;
            }).ToList();
            if (!_SelDictionary.ContainsKey(iClass) && _SelKeep)
            {
                _SelDictionary.Add(iClass, -99);
            }
        }
        if (cellsMaxCountInPage != 0 && _KeepPage)
        {//設定可顯示頁數
            _pageCount = _aCurShowGridItem.Count / cellsMaxCountInPage + 1;
            if (_pageCount == 1) { _pageCount = _showPageCount; }
        }
        MessageBox.DEBUG("f_Sort " + _aCurShowGridItem.Count);
        if (bUpdate)
        {
            this.updatePage();
            //重新调用item回调
            this.UpdateItem();

            rollPosByPageByIndex(0);
        }
    }

    /// <summary>
    /// 创建一个页的内容
    /// </summary>
    /// <param name="prefab">预制件</param>
    /// <param name="createPageCount">可显示的页数</param>
    private void createPageItem(GameObject prefab, int createPageCount)
    {
        if (createPageCount <= 0) return;
        if (this.pageList == null)
            this.pageList = new List<GameObject>();

        if (this.pageItemList == null)
            this.pageItemList = new List<List<GridItem>>();

        if (pageList.Count >= createPageCount)
        {
            return;
        }
        int iCreateNum = createPageCount - pageList.Count;
        for (int i = 0; i < iCreateNum; ++i)
        {
            GameObject pageContainer = new GameObject();
            pageContainer.AddComponent<RectTransform>();
            pageContainer.AddComponent<CanvasRenderer>();
            pageContainer.transform.SetParent(this.content.gameObject.transform);
            pageContainer.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            pageContainer.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            pageContainer.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            pageContainer.GetComponent<RectTransform>().sizeDelta = this.pageSize;
            pageContainer.transform.localScale = new Vector3(1, 1, 1);

            List<GridItem> list = new List<GridItem>();
            this.pageItemList.Add(list);

            //设置每一页的位置
            pageContainer.name = "page" + i;
            this.pageList.Add(pageContainer);

            int rows = 1;       //修正原显示顶行BUG
            int columns = 0;
            for (int j = 0; j < this.cellsMaxCountInPage; ++j)
            {
                GameObject item = MonoBehaviour.Instantiate(prefab, new Vector3(0, 0), new Quaternion()) as GameObject;
                item.transform.SetParent(pageContainer.transform);
                item.transform.localScale = new Vector3(1, 1, 1);
                //排列先横向再纵向
                float x = columns * (this._iItemWidth + this.gapH);
                float y = rows * -(this._iItemHeight + this.gapV);
                item.transform.localPosition = new Vector3(x, y);
                columns++;
                if (columns == this._iColumns)
                {
                    columns = 0;
                    rows++;
                }

                GridItem tGridItem = item.GetComponent<GridItem>();
                if (tGridItem == null)
                {
                    tGridItem = item.AddComponent<GridItem>();
                }
                item.name = pageContainer.name + "_" + "Item" + j;
                ccUIEventListener.Get(item).f_RegClick(GOClickHandle, tGridItem, null, "");
                list.Add(tGridItem);

            }
        }
    }

    /// <summary>
    /// 重新设置数据
    /// </summary>
    /// <param name="cellCount">当前数据列表的数量</param>
    void CreateData()
    {
        //for (int i = 0; i < _cellCount; i++ )
        //{
        //    GridItemDT tGridItemDT = new GridItemDT(CallBack_OnItemClick);
        //    if (sourceData == null)
        //    {
        //        tGridItemDT.f_InitData("iIndex:" + i);
        //    }
        //    else
        //    { 
        //        tGridItemDT.f_InitData("iIndex:" + i, sourceData[i]);
        //    }
        //    _aGridItem.Add(tGridItemDT);
        //}

        this.contentRectTf.DOKill();
        this.firstPagePos = this.getFirstPagePos();
        int prevPageCount = this._pageCount;
        //计算页数
        if (cellCount % this.cellsMaxCountInPage == 0)
        {
            this._pageCount = cellCount / this.cellsMaxCountInPage;
        }
        else
            this._pageCount = cellCount / this.cellsMaxCountInPage + 1;
        //计算最后一页的格子数量
        this.lastPageItemCount = cellCount % this.cellsMaxCountInPage;
        if (lastPageItemCount == 0) lastPageItemCount = this.cellsMaxCountInPage;
        int curLastPageIndex = this.curPageIndex + this.showPageCount - 1;
        int lastPageIndex = this._pageCount - 1;
        if (this.curPageIndex > 0 && curLastPageIndex > lastPageIndex)
        {
            //当前不在第一页 并且 最后索引位置溢出了
            int overCount = curLastPageIndex - lastPageIndex;
            this.curPageIndex -= overCount;
            //防止去除溢出后 索引为负数。
            if (this.curPageIndex < 0) this.curPageIndex = 0;
            if (this.curPageIndex == 0)
            {
                this.firstPagePos = new Vector2();
            }
            else
            {
                //补全位置
                if (!this.isHorizontal)
                    this.firstPagePos.y += this.pageSize.y * overCount;
                else
                    this.firstPagePos.x -= this.pageSize.x * overCount;
            }
        }
        //删除多余的页数
        this.removeOverPage();
        //获取最新边界
        if (this._pageCount < this.showPageCount)
            this.showPageCount = this._pageCount;
        int creatPageCount = this.showPageCount - prevPageCount;
        //this.createPageItem(this.itemPrefab, creatPageCount);
        this.createPageItem(this.itemPrefab, showPageCount);
        this.updateContentSize();
        this.updateBorder();
        this.layoutPage();
        this.fixContentPos();
        this.updatePageItemActive();

    }

    /// <summary>
    /// 更新滚动内容的大小
    /// </summary>
    private void updateContentSize()
    {
        Vector2 size;
        int iSizePage = _aCurShowGridItem.Count % cellsMaxCountInPage;
        if (this.isHorizontal)
            size = new Vector2(this.pageSize.x * this._pageCount,
                               this.pageSize.y);
        else
            size = new Vector2(this.pageSize.x,
                               this.pageSize.y * this._pageCount);
        /*if (this.isHorizontal)
        {
            int iEndPage = iSizePage * _iRows;
            size = new Vector2(pageSize.x * (_pageCount) + iEndPage, pageSize.y);
        }
        else
        {
            int iEndPage = iSizePage * _iColumns;
            size = new Vector2(pageSize.x, pageSize.y * (_pageCount) + iEndPage);
        }*/
        this.contentRectTf.sizeDelta = size;
    }


    /// <summary>
    /// 修正content的位置
    /// </summary>
    private void fixContentPos()
    {
        if (!this.isHorizontal)
        {
            //防止数量减少后content的位置在遮罩上面
            if (this.contentRectTf.sizeDelta.y <= this.pageSize.y)
            {
                //如果高度不够但content顶部超过scroll的顶部则content顶部归零对齐
                if (this.contentRectTf.localPosition.y > 0)
                    this.contentRectTf.localPosition = new Vector3(this.contentRectTf.localPosition.x, 0);
            }
            else
            {
                //如果高度足够但content底部超过scroll的底部则content底部对齐scroll的底部
                if (this.contentRectTf.localPosition.y - this.contentRectTf.sizeDelta.y > -this.pageSize.y)
                    this.contentRectTf.localPosition = new Vector3(this.contentRectTf.localPosition.x,
                                                                    -this.pageSize.y + this.contentRectTf.sizeDelta.y);
            }
        }
        else
        {
            //防止数量减少后content的位置在遮罩左面
            if (this.contentRectTf.sizeDelta.x <= this.pageSize.x)
            {
                if (this.contentRectTf.localPosition.x < 0)
                    this.contentRectTf.localPosition = new Vector3(0, this.contentRectTf.localPosition.y);
            }
            else
            {
                if (this.contentRectTf.localPosition.x + this.contentRectTf.sizeDelta.x < this.pageSize.x)
                    this.contentRectTf.localPosition = new Vector3(this.pageSize.x - this.contentRectTf.sizeDelta.x,
                                                                   this.contentRectTf.localPosition.y);
            }
        }
    }

    /// <summary>
    /// 拖动结束
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnEndDrag(PointerEventData eventData)
    {
        this.sr.StopMovement();
        GameObject pageGo = this.getPageGoByPageIndex(this.curPageIndex);
        //计算当前页未拖动时的位置
        Vector2 beforeDragPagePos = new Vector2(-this.curPageIndex * this.pageSize.x, this.curPageIndex * this.pageSize.y);
        //获取拖动后的位置
        Vector2 afterDragPagePos = this.content.transform.localPosition;
        if (this.isHorizontal)
        {
            //横向
            if (afterDragPagePos.x < beforeDragPagePos.x)
            {
                //往左拖动
                if (afterDragPagePos.x < beforeDragPagePos.x - this.pageSize.x * .5f)
                    this.contentRectTf.DOLocalMoveX(beforeDragPagePos.x - this.pageSize.x - .01f, .4f); //翻页
                else
                    this.contentRectTf.DOLocalMoveX(beforeDragPagePos.x, .4f); //回滚
            }
            else if (afterDragPagePos.x > beforeDragPagePos.x)
            {
                //往右拖动
                if (afterDragPagePos.x > beforeDragPagePos.x + this.pageSize.x * .5f)
                    this.contentRectTf.DOLocalMoveX(beforeDragPagePos.x + this.pageSize.x + .01f, .4f); //翻页
                else
                    this.contentRectTf.DOLocalMoveX(beforeDragPagePos.x, .4f); //回滚
            }
        }
        else
        {
            //纵向
            if (afterDragPagePos.y > beforeDragPagePos.y)
            {
                //向上    拖動的有效範圍可以調成0.3f
                if (afterDragPagePos.y > beforeDragPagePos.y + this.pageSize.y * 0.3f)
                    this.contentRectTf.DOLocalMoveY(beforeDragPagePos.y + this.pageSize.y + .01f, .4f); //翻页
                else
                    this.contentRectTf.DOLocalMoveY(beforeDragPagePos.y, .4f); //回滚
            }
            else if (afterDragPagePos.y < beforeDragPagePos.y)
            {
                //向下
                if (afterDragPagePos.y < beforeDragPagePos.y - this.pageSize.y * 0.3f)
                    this.contentRectTf.DOLocalMoveY(beforeDragPagePos.y - this.pageSize.y - .01f, .4f); //翻页
                else
                    this.contentRectTf.DOLocalMoveY(beforeDragPagePos.y, .4f); //回滚
            }
        }
    }

    /// <summary>
    /// 循环page的滚动
    /// </summary>
    private void updatePage()
    {
        if (this.pageList == null)
        {
            return;
        }
        for (int i = 0; i < this.pageList.Count; ++i)
        {
            List<GridItem> itemList = this.pageItemList[i];
            GameObject pageGo = this.pageList[i];
            Transform pageGoTr = pageGo.transform;
            Vector2 pagePos = scroll.transform.InverseTransformPoint(pageGoTr.position);
            if (this.isHorizontal)
            {
                //if (pagePos.x < this.left && this.curPageIndex < this._pageCount - this.showPageCount)
                if (pagePos.x < this.left && this.curPageIndex < this._pageCount - 1)
                {
                    //向左循环
                    GameObject lastPageGo = this.pageList[this.pageList.Count - 1];
                    this.pageList.RemoveAt(i);
                    pageGoTr.localPosition = new Vector3(lastPageGo.transform.localPosition.x + this.pageSize.x,
                                                         pageGoTr.localPosition.y);
                    this.pageList.Add(pageGo);
                    pageGo.name = "page" + (this.curPageIndex + this.showPageCount);

                    this.pageItemList.RemoveAt(i);
                    this.pageItemList.Add(itemList);

                    this.curPageIndex++;

                    if (this.curPageIndex == this._pageCount - this.showPageCount)
                        this.setPageItemActive(itemList);

                    this.UpdateItem();
                    break;
                }
                else if (pagePos.x > this.right && this.curPageIndex > 0)
                {
                    //向右循环
                    GameObject firstPageGo = this.pageList[0];
                    this.pageList.RemoveAt(i);
                    pageGoTr.localPosition = new Vector3(firstPageGo.transform.localPosition.x - this.pageSize.x,
                                                         pageGoTr.localPosition.y);
                    this.pageList.Insert(0, pageGo);
                    pageGo.name = "page" + (this.curPageIndex + this.showPageCount);

                    this.pageItemList.RemoveAt(i);
                    this.pageItemList.Insert(0, itemList);

                    if (this.curPageIndex == this._pageCount - this.showPageCount)
                        this.setPageItemActive(itemList, false);
                    this.curPageIndex--;

                    this.UpdateItem();
                    break;
                }
            }
            else
            {
                //if (pagePos.y > this.top && this.curPageIndex < this._pageCount - this.showPageCount)
                if (pagePos.y > this.top && this.curPageIndex < this._pageCount - 1)
                {
                    //向上循环
                    GameObject lastPageGo = this.pageList[this.pageList.Count - 1];
                    this.pageList.RemoveAt(i);
                    pageGoTr.localPosition = new Vector3(pageGoTr.localPosition.x, lastPageGo.transform.localPosition.y - this.pageSize.y);
                    this.pageList.Add(pageGo);
                    pageGo.name = "page" + (this.curPageIndex + this.showPageCount);

                    this.pageItemList.RemoveAt(i);
                    this.pageItemList.Add(itemList);

                    this.curPageIndex++;
                    if (this.curPageIndex == this._pageCount - this.showPageCount)
                        this.setPageItemActive(itemList);

                    //重新调用item回调
                    this.UpdateItem();
                    break;
                }
                else if (pagePos.y < this.bottom && this.curPageIndex > 0)
                {
                    //向下循环
                    GameObject firstPageGo = this.pageList[0];
                    this.pageList.RemoveAt(i);
                    pageGoTr.localPosition = new Vector3(pageGoTr.localPosition.x,
                                                         firstPageGo.transform.localPosition.y + this.pageSize.y);
                    this.pageList.Insert(0, pageGo);
                    pageGo.name = "page" + (this.curPageIndex + this.showPageCount);

                    this.pageItemList.RemoveAt(i);
                    this.pageItemList.Insert(0, itemList);
                    if (this.curPageIndex == this._pageCount - this.showPageCount)
                        this.setPageItemActive(itemList, false);
                    this.curPageIndex--;

                    //重新调用item回调
                    this.UpdateItem();
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 执行item回调
    /// </summary>
    /// <param name="isReload"></param>
    private void UpdateItem(bool isReload = false)
    {
        if (this.pageList == null || this.pageList.Count == 0) return;

        if (!_bLockDrag)
        {
            if (this.cellCount <= cellsMaxCountInPage)
            {
                m_ScrollRect.enabled = false;
            }
            else
            {
                m_ScrollRect.enabled = true;
            }
        }
        if (_aCurSelItem.Count != 0)
        {
            for (int i = 0; i < _aCurSelItem.Count; i++)
            {
                _aCurSelItem[i].f_UnSelect();
            }
            _aCurSelItem.Clear();
        }

        int pageIndex = 0;
        for (int i = this.curPageIndex; i < this.curPageIndex + this.showPageCount; ++i)
        {
            if (this.pageItemList[pageIndex] != null)
            {
                List<GridItem> itemList = this.pageItemList[pageIndex];
                int count = itemList.Count;
                for (int j = 0; j < count; ++j)
                {
                    int itemIndex = i * this.cellsMaxCountInPage + j;
                    GridItem tGridItem = itemList[j];
                    //多余的item不做回调
                    if (itemIndex <= this.cellCount - 1)
                    {
                        tGridItem.f_UpdateUI(_aCurShowGridItem[itemIndex]);
                        tGridItem.f_Show();
                        //if (this.m_updateItem != null)
                        //    this.m_updateItem.Invoke(item, itemIndex, i, isReload);

                        if (_SelKeep)
                        {
                            if (_SelDictionary.ContainsKey(tGridItem.f_GetGridItemDT().f_GetClass()) && _iCurClass != -99)
                            {
                                if (_SelDictionary[tGridItem.f_GetGridItemDT().f_GetClass()] == tGridItem.f_GetGridItemDT().m_iId)
                                {
                                    GOClickHandle(null, tGridItem, null);
                                }
                            }
                        }
                    }
                    else
                    {
                        tGridItem.f_UnShow();
                    }
                }
                pageIndex++;
            }
        }
    }

    /// <summary>
    /// 设置页数内的item显示或者隐藏
    /// </summary>
    /// <param name="pageItemList">一页里的item列表</param>
    /// <param name="hide">是否隐藏</param>
    private void setPageItemActive(List<GridItem> pageItemList, bool hide = true)
    {
        if (this.lastPageItemCount == 0) return;
        int count = pageItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            if (hide)
            {
                if (i > this.lastPageItemCount - 1)
                {
                    //pageItemList[i].gameObject.SetActive(false);
                    pageItemList[i].f_UnShow();
                }
                else
                {
                    //pageItemList[i].gameObject.SetActive(true);
                    pageItemList[i].f_Show();
                }
            }
            else
            {
                //pageItemList[i].gameObject.SetActive(true);
                pageItemList[i].f_Show();
            }
        }
    }

    /// <summary>
    /// 更新一页内item是否可以显示
    /// </summary>
    private void updatePageItemActive()
    {
        if (this.pageList == null) return;
        if (this.pageList.Count == 0) return;
        for (int i = 0; i < this.showPageCount; ++i)
        {
            List<GridItem> itemList = this.pageItemList[i];
            for (int j = 0; j < this.cellsMaxCountInPage; ++j)
            {
                GridItem item = itemList[j];
                if (this._pageCount <= this.showPageCount) //排数在一屏以内
                {
                    if (i < this.showPageCount - 1) //前几排全部显示
                    {
                        //item.gameObject.SetActive(true);
                        item.f_Show();
                    }
                    else
                    {
                        if (j <= this.lastPageItemCount - 1)  //最后一排判断可显示的item
                        {
                            //item.gameObject.SetActive(true);
                            item.f_Show();
                        }
                        else
                        {
                            //item.gameObject.SetActive(false);
                            item.f_UnShow();
                        }
                    }
                }
                else //排数超过一屏
                {
                    //判断最后一排是否在显示范围内
                    if (this.curPageIndex < this._pageCount - this.showPageCount)
                    {
                        //item.gameObject.SetActive(true);
                        item.f_Show();
                    }
                    else
                    {
                        if (i < this.showPageCount - 1) //前几排全部显示
                        {
                            //item.gameObject.SetActive(true);
                            item.f_Show();
                        }
                        else
                        {
                            if (j <= this.lastPageItemCount - 1)  //最后一排判断可显示的item
                            {
                                //item.gameObject.SetActive(true);
                                item.f_Show();
                            }
                            else
                            {
                                //item.gameObject.SetActive(false);
                                item.f_UnShow();
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 获取第一页的位置
    /// </summary>
    private Vector2 getFirstPagePos()
    {
        //保存上一次第一个item的位置
        if (this.pageList == null || this.pageList.Count == 0)
        {
            //默认从0，0开始
            return new Vector2();
        }
        GameObject pageGo = this.pageList[0];
        return pageGo.transform.localPosition;
    }

    /// <summary>
    /// page布局
    /// </summary>
    /// <returns></returns>
    private void layoutPage()
    {
        if (this.pageList == null) return;
        int count = this.pageList.Count;
        for (int i = 0; i < count; ++i)
        {
            GameObject pageGo = this.pageList[i];
            if (!this.isHorizontal)
                pageGo.transform.localPosition = new Vector3(0, this.firstPagePos.y - this.pageSize.y * i);
            else
                pageGo.transform.localPosition = new Vector3(this.firstPagePos.x + this.pageSize.x * i, 0);
        }
    }

    /// <summary>
    /// 删除多余的页数
    /// </summary>
    private void removeOverPage()
    {
        if (this.pageList == null ||
            this.pageList.Count == 0) return;
        if (this._pageCount >= this.showPageCount) return;
        for (int i = this.showPageCount - 1; i >= this._pageCount; --i)
        {
            List<GridItem> itemList = this.pageItemList[i];
            int count = itemList.Count;
            for (int j = count - 1; j >= 0; --j)
            {
                GridItem item = itemList[j];
                GameObject.Destroy(item.gameObject);
                itemList.RemoveAt(j);
            }
            this.pageItemList.RemoveAt(i);
            GameObject.Destroy(this.pageList[i]);
            this.pageList.RemoveAt(i);
        }
    }

    /// <summary>
    /// 更新边界
    /// </summary>
    /// <returns></returns>
    void updateBorder()
    {
        //上下
        this.top = this.pageSize.y;
        this.bottom = -this.pageSize.y * (this.showPageCount - 1);
        //左右
        this.left = -this.pageSize.x;
        this.right = this.pageSize.x * (this.showPageCount - 1);
    }

    /// <summary>
    /// 根据页数返回页数容器
    /// </summary>
    /// <param name="page">页数索引</param>
    /// <returns>页数显示容器</returns>
    private GameObject getPageGoByPageIndex(int page)
    {
        if (this.pageList == null ||
            this.pageList.Count == 0 ||
            page > this.pageList.Count - 1) return null;
        return this.pageList[page];
    }

    /// <summary>
    /// 根据页数滚动到相应位置
    /// </summary>
    /// <param name="page">页数索引</param>
    public void rollPosByPage(int page)
    {
        if (this.pageList == null ||
            this.pageList.Count == 0) return;
        this.sr.StopMovement();
        if (page < 0) page = 0;
        if (page > this._pageCount - 1) page = this._pageCount - 1;
        this.curPageIndex = page;
        //计算出第一个索引是多少， 因为第一个curLineIndex不一定是targetLineIndex 
        if (this.curPageIndex + this.showPageCount > this._pageCount)
            this.curPageIndex -= page + this._pageCount - this.showPageCount;
        Vector3 contentPos = this.contentRectTf.localPosition;
        if (!this.isHorizontal)
        {
            this.firstPagePos.y = -this.pageSize.y * this.curPageIndex; //算出移动的距离
            contentPos.y = this.pageSize.y * page;
        }
        else
        {
            this.firstPagePos.x = this.pageSize.x * this.curPageIndex; //算出移动的距离
            contentPos.x = -this.pageSize.x * page;
        }
        this.contentRectTf.localPosition = contentPos;
        this.UpdateItem(true);
        this.layoutPage();
        this.updatePageItemActive();
        this.fixContentPos();
    }

    /// <summary>
    /// 根据item索引跳转到页数
    /// </summary>
    /// <param name="itemIndex">item的索引</param>
    public void rollPosByPageByIndex(int itemIndex)
    {
        if (itemIndex < 0) itemIndex = 0;
        if (itemIndex > this.cellCount - 1) itemIndex = this.cellCount - 1;
        int page = Mathf.CeilToInt((itemIndex + 1) / this.cellsMaxCountInPage);
        this.rollPosByPage(page);
    }

    //// Update is called once per frame
    //void f_Update () 
    //   {
    //       this.updatePage();
    //       //重新调用item回调
    //       this.UpdateItem();
    //}

    /// <summary>
    /// 点击Item处理
    /// </summary>
    /// <param name="go"></param>
    private void GOClickHandle(GameObject go, object obj1, object obj2)
    {
        GridItem tGridItem = (GridItem)obj1;
        if (tGridItem.f_GetGridItemDT() == null)
        {
            return;
        }
        if (_iMaxSelNum == 1)
        {
            long iId = tGridItem.f_GetGridItemDT().m_iId;
            if (_SelKeep)
            {
                _SelDictionary[tGridItem.f_GetGridItemDT().f_GetClass()] = iId;
            }
            if (_aCurSelItem.Count == 0)
            {
                _aCurSelItem.Add(tGridItem);
                tGridItem.f_Select();
                _CallBack_OnClick(obj1);
            }
            else
            {
                if (_aCurSelItem[0] == tGridItem)
                {
                    _CallBack_OnClick(obj1);
                }
                else
                {
                    _aCurSelItem[0].f_UnSelect();
                    _aCurSelItem.Clear();

                    _aCurSelItem.Add(tGridItem);
                    tGridItem.f_Select();
                    _CallBack_OnClick(obj1);
                }
            }
        }
        else
        {
            //检查是否在忆选择列表，并进行相应的选择和取消选择的逻辑处理
            GridItem tFindItem = _aCurSelItem.Find(delegate (GridItem tItem)
            {
                if (tItem.f_GetGridItemDT() == tGridItem.f_GetGridItemDT())
                {
                    return true;
                }
                return false;
            });
            if (tFindItem == null)
            {
                if (_iMaxSelNum <= _aCurSelItem.Count)
                {
                    return;
                }
                _aCurSelItem.Add(tGridItem);
                tGridItem.f_Select();
                _CallBack_OnClick(obj1);
            }
            else
            {
                tFindItem.f_UnSelect();
                _aCurSelItem.Remove(tFindItem);
            }
        }
    }

    #region 對外取得
    public List<GridItem> f_GetCurSelItem()
    {
        return _aCurSelItem;
    }

    public void f_IsSelItem(int iClass, long iId)
    {
        if (_SelKeep)
        {
            if (_SelDictionary.ContainsKey(iClass))
            {
                for (int i = 0; i < _aCurSelItem.Count; i++)
                {
                    if (iId == _aCurSelItem[i].f_GetGridItemDT().m_iId)
                    {
                        GOClickHandle(null, _aCurSelItem[i], null);
                    }
                }
            }
        }
    }

    public void f_ClearCurSelItem()
    {
        for (int i = 0; i < _aCurSelItem.Count; i++)
        {
            _aCurSelItem[i].f_UnSelect();
            if (_SelKeep)
            {
                _SelDictionary[_aCurSelItem[i].f_GetGridItemDT().f_GetClass()] = -99;
            }
        }
        _aCurSelItem.Clear();
    }

    public void f_ClearIsSelItem(int iClass)
    {
        if (_SelDictionary.ContainsKey(iClass))
        {
            _SelDictionary[iClass] = -99;
        }
    }

    public List<GameObject> f_GetPageList()
    {
        return pageList;
    }

    public List<GridItem> f_GetPageItem(int iIndex)
    {
        return pageItemList[iIndex];
    }

    public void f_SetSubItemList(long iId, bool bAdd)
    {
        if (bAdd)
        {
            _SubItemList.Add(iId);
        }
        else
        {
            _SubItemList.Remove(iId);
        }
    }
    #endregion
}