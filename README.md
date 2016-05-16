# MvcHwDay01
Day01 Homework for ASP.NET MVC 5 course instructed by Demo (20160409 .. 20160507)

# 操作說明:
1. 在首頁有 3 個 Button 可以使用.
(1) 新增記帳資料(分頁)(需登入)
(2) 修改記帳資料(分頁)(含網址列年月收支查詢)(需管理權限)
(3) 修改記帳資料(分頁)(含人工表單條件查詢)(需管理權限)

# 目前問題
## 2016.05.14:
1. 主層 BillController.cs 的 "新增記帳資料(分頁)(需登入)" 功能, 在點按下方頁碼時, 會作2次 HttpGet 的動作.
2. Area 層 BillController.cs 的 "修改記帳資料(分頁)(含人工表單條件查詢)(需管理權限)" 功能, 在點按下方頁碼時, 不會將 PageIndex 傳回

# 待辦事項
## 2016.05.16:
1. 金額用選的
2. 實作 RSS Action Result


