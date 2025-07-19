# Hướng dẫn xây dựng Dashboard Power BI cho DNA Testing System

## Tổng quan

Tài liệu này hướng dẫn cách tạo Dashboard Power BI để phân tích dữ liệu từ hệ thống DNA Testing System sử dụng các API endpoints có sẵn.

## 📊 API Endpoints có sẵn

### 1. AppointmentsTienDM Controller APIs

| Endpoint                            | Method | Mô tả                              | Dữ liệu trả về                                |
| ----------------------------------- | ------ | ---------------------------------- | --------------------------------------------- |
| `/api/AppointmentsTienDM`           | GET    | Lấy tất cả appointments (có OData) | List<AppointmentsTienDmDto>                   |
| `/api/AppointmentsTienDM/paginated` | GET    | Lấy appointments có phân trang     | PaginationResult<List<AppointmentsTienDmDto>> |
| `/api/AppointmentsTienDM/{id}`      | GET    | Lấy appointment theo ID            | AppointmentsTienDmDto                         |
| `/api/AppointmentsTienDM/search`    | POST   | Tìm kiếm nâng cao có phân trang    | PaginationResult<List<AppointmentsTienDmDto>> |

### 2. Dữ liệu từ AppointmentsTienDmDto

```json
{
  "appointmentsTienDmid": "int",
  "userAccountId": "int",
  "servicesNhanVtid": "int",
  "appointmentStatusesTienDmid": "int",
  "appointmentDate": "DateOnly",
  "appointmentTime": "TimeOnly",
  "samplingMethod": "string",
  "address": "string",
  "contactPhone": "string",
  "notes": "string",
  "createdDate": "DateTime",
  "modifiedDate": "DateTime",
  "totalAmount": "decimal",
  "isPaid": "bool",
  "username": "string",
  "serviceName": "string",
  "statusName": "string"
}
```

## 🔧 Thiết lập Power BI

### Bước 1: Cài đặt Power BI Desktop

1. Tải và cài đặt Power BI Desktop từ Microsoft Store hoặc trang chủ Microsoft
2. Đăng nhập bằng tài khoản Microsoft/Office 365

### Bước 2: Kết nối với Web API

#### 2.1 Tạo Data Source

1. Mở Power BI Desktop
2. Chọn **Get Data** → **Web**
3. Nhập URL API: `http://localhost:8080/api/AppointmentsTienDM`

#### 2.2 Xử lý Authentication (nếu cần)

```javascript
// Nếu API yêu cầu Bearer Token
let
    apiUrl = "http://localhost:8080/api/AppointmentsTienDM",
    headers = [
        #"Authorization" = "Bearer YOUR_TOKEN_HERE",
        #"Content-Type" = "application/json"
    ],
    response = Web.Contents(apiUrl, [Headers=headers]),
    jsonData = Json.Document(response)
in
    jsonData
```

#### 2.3 Sử dụng OData Query (Khuyến nghị)

```
http://localhost:8080/api/AppointmentsTienDM?$select=appointmentsTienDmid,appointmentDate,totalAmount,isPaid,statusName,serviceName&$orderby=appointmentDate desc
```

### Bước 3: Transform Data với Power Query

#### 3.1 Làm sạch dữ liệu

```m
let
    Source = Web.Contents("http://localhost:8080/api/AppointmentsTienDM"),
    #"Parsed JSON" = Json.Document(Source),
    #"Converted to Table" = Table.FromList(#"Parsed JSON", Splitter.SplitByNothing(), null, null, ExtraValues.Error),
    #"Expanded Column" = Table.ExpandRecordColumn(#"Converted to Table", "Column1",
        {"appointmentsTienDmid", "appointmentDate", "appointmentTime", "totalAmount", "isPaid", "statusName", "serviceName", "samplingMethod", "createdDate"},
        {"ID", "AppointmentDate", "AppointmentTime", "TotalAmount", "IsPaid", "Status", "Service", "SamplingMethod", "CreatedDate"}),
    #"Changed Type" = Table.TransformColumnTypes(#"Expanded Column", {
        {"AppointmentDate", type date},
        {"AppointmentTime", type time},
        {"TotalAmount", Currency.Type},
        {"IsPaid", type logical},
        {"CreatedDate", type datetime}
    })
in
    #"Changed Type"
```

#### 3.2 Tạo Date Table (quan trọng cho Time Intelligence)

```m
let
    StartDate = #date(2024, 1, 1),
    EndDate = #date(2025, 12, 31),
    DateList = List.Dates(StartDate, Duration.Days(EndDate - StartDate) + 1, #duration(1, 0, 0, 0)),
    DateTable = Table.FromList(DateList, Splitter.SplitByNothing(), {"Date"}),
    #"Added Year" = Table.AddColumn(DateTable, "Year", each Date.Year([Date])),
    #"Added Month" = Table.AddColumn(#"Added Year", "Month", each Date.Month([Date])),
    #"Added Month Name" = Table.AddColumn(#"Added Month", "MonthName", each Date.MonthName([Date])),
    #"Added Quarter" = Table.AddColumn(#"Added Month Name", "Quarter", each "Q" & Text.From(Date.QuarterOfYear([Date]))),
    #"Added Day of Week" = Table.AddColumn(#"Added Quarter", "DayOfWeek", each Date.DayOfWeekName([Date]))
in
    #"Added Day of Week"
```

## 📈 Dashboard với 6 Loại Chart Cơ Bản

### Dashboard Layout

```
┌─────────────────────────────────────────────────────────────────┐
│                    DNA TESTING SYSTEM DASHBOARD                 │
├─────────────────────────────────────────────────────────────────┤
│  ┌─────────────────────────┐ ┌─────────────────────────┐       │
│  │   1. Line Chart         │ │   2. Bar Chart          │       │
│  │   Appointments Trend    │ │   Revenue by Service    │       │
│  └─────────────────────────┘ └─────────────────────────┘       │
├─────────────────────────────────────────────────────────────────┤
│  ┌─────────────────────────┐ ┌─────────────────────────┐       │
│  │   3. Pie Chart          │ │   4. Column Chart       │       │
│  │   Status Distribution   │ │   Monthly Revenue       │       │
│  └─────────────────────────┘ └─────────────────────────┘       │
├─────────────────────────────────────────────────────────────────┤
│  ┌─────────────────────────┐ ┌─────────────────────────┐       │
│  │   5. Donut Chart        │ │   6. Table/Matrix       │       │
│  │   Payment Status        │ │   Detailed Data         │       │
│  └─────────────────────────┘ └─────────────────────────┘       │
└─────────────────────────────────────────────────────────────────┘
```

## 📊 6 Loại Chart Cơ Bản

### 1. Line Chart - Xu hướng Appointments theo thời gian

**Dữ liệu cần:**

- X-Axis: `appointmentDate`
- Y-Axis: Count of appointments
- Có thể thêm Legend: `statusName`

**Power Query để chuẩn bị dữ liệu:**

```m
// Tạo bảng theo ngày
Appointments_Daily =
Table.Group(
    Appointments,
    {"AppointmentDate"},
    {{"Count", each Table.RowCount(_), Int64.Type}}
)
```

**DAX Measure:**

```dax
Daily Appointments = COUNTROWS(Appointments)
```

---

### 2. Bar Chart - Doanh thu theo từng Service

**Dữ liệu cần:**

- Axis: `serviceName`
- Values: Sum of `totalAmount`

**DAX Measure:**

```dax
Revenue by Service =
CALCULATE(
    SUM(Appointments[TotalAmount]),
    ALLEXCEPT(Appointments, Appointments[ServiceName])
)
```

---

### 3. Pie Chart - Phân bố Status của Appointments

**Dữ liệu cần:**

- Legend: `statusName`
- Values: Count of appointments

**DAX Measure:**

```dax
Status Count =
CALCULATE(
    COUNTROWS(Appointments),
    ALLEXCEPT(Appointments, Appointments[StatusName])
)

Status Percentage =
DIVIDE(
    [Status Count],
    CALCULATE(COUNTROWS(Appointments), ALL(Appointments[StatusName])),
    0
) * 100
```

---

### 4. Column Chart - Doanh thu theo tháng

**Dữ liệu cần:**

- X-Axis: Month từ `appointmentDate`
- Y-Axis: Sum of `totalAmount`

**Power Query để tạo cột Month:**

```m
#"Added Month" = Table.AddColumn(Appointments, "Month", each Date.MonthName([AppointmentDate]))
```

**DAX Measure:**

```dax
Monthly Revenue =
CALCULATE(
    SUM(Appointments[TotalAmount]),
    DATESMTD('Calendar'[Date])
)
```

---

### 5. Donut Chart - Tỷ lệ thanh toán (Paid vs Unpaid)

**Dữ liệu cần:**

- Legend: `isPaid` (True/False)
- Values: Count of appointments

**Power Query để format isPaid:**

```m
#"Added Payment Status" = Table.AddColumn(Appointments, "PaymentStatus",
    each if [IsPaid] = true then "Paid" else "Unpaid")
```

**DAX Measures:**

```dax
Paid Count =
CALCULATE(
    COUNTROWS(Appointments),
    Appointments[IsPaid] = TRUE
)

Unpaid Count =
CALCULATE(
    COUNTROWS(Appointments),
    Appointments[IsPaid] = FALSE
)

Payment Rate =
DIVIDE([Paid Count], [Paid Count] + [Unpaid Count], 0) * 100
```

---

### 6. Table/Matrix - Bảng dữ liệu chi tiết

**Columns hiển thị:**

- `appointmentDate`
- `username`
- `serviceName`
- `statusName`
- `totalAmount`
- `isPaid`
- `samplingMethod`

**Conditional Formatting:**

```dax
// Màu cho Payment Status
Payment Color =
IF(
    SELECTEDVALUE(Appointments[IsPaid]) = TRUE,
    "Green",
    "Red"
)

// Màu cho Total Amount
Amount Color =
SWITCH(
    TRUE(),
    SELECTEDVALUE(Appointments[TotalAmount]) >= 1000000, "Green",
    SELECTEDVALUE(Appointments[TotalAmount]) >= 500000, "Orange",
    "Red"
)
```

## � Cấu hình từng Chart trong Power BI

### 1. Tạo Line Chart - Appointments Trend

**Bước 1:** Drag visual "Line Chart" vào canvas
**Bước 2:** Cấu hình fields:

```
✓ X-axis: AppointmentDate (Date Hierarchy)
✓ Y-axis: Daily Appointments (measure)
✓ Legend: StatusName (optional)
```

### 2. Tạo Bar Chart - Revenue by Service

**Bước 1:** Drag visual "Clustered Bar Chart"
**Bước 2:** Cấu hình fields:

```
✓ Y-axis: ServiceName
✓ X-axis: Revenue by Service (measure)
✓ Sort: Descending by value
```

### 3. Tạo Pie Chart - Status Distribution

**Bước 1:** Drag visual "Pie Chart"
**Bước 2:** Cấu hình fields:

```
✓ Legend: StatusName
✓ Values: Status Count (measure)
✓ Enable data labels với percentages
```

### 4. Tạo Column Chart - Monthly Revenue

**Bước 1:** Drag visual "Clustered Column Chart"
**Bước 2:** Cấu hình fields:

```
✓ X-axis: Month (từ AppointmentDate)
✓ Y-axis: Monthly Revenue (measure)
✓ Enable trend line
```

### 5. Tạo Donut Chart - Payment Status

**Bước 1:** Drag visual "Donut Chart"
**Bước 2:** Cấu hình fields:

```
✓ Legend: PaymentStatus (Paid/Unpaid)
✓ Values: Count of appointments
✓ Inner radius: 50%
```

### 6. Tạo Table - Detailed Data

**Bước 1:** Drag visual "Table"
**Bước 2:** Add columns:

```
✓ AppointmentDate
✓ Username
✓ ServiceName
✓ StatusName
✓ TotalAmount
✓ IsPaid
```

## 🎯 Kết nối API và Load Data

### Data Source Configuration

**URL chính:** `http://localhost:8080/api/AppointmentsTienDM`

**Power Query M Code:**

```m
let
    Source = Web.Contents("http://localhost:8080/api/AppointmentsTienDM"),
    JsonData = Json.Document(Source),
    TableData = Table.FromList(JsonData, Splitter.SplitByNothing(), null, null, ExtraValues.Error),
    ExpandedData = Table.ExpandRecordColumn(TableData, "Column1",
        {"appointmentsTienDmid", "appointmentDate", "totalAmount", "isPaid", "statusName", "serviceName", "username", "samplingMethod"},
        {"ID", "AppointmentDate", "TotalAmount", "IsPaid", "StatusName", "ServiceName", "Username", "SamplingMethod"}),
    TypedData = Table.TransformColumnTypes(ExpandedData, {
        {"AppointmentDate", type date},
        {"TotalAmount", Currency.Type},
        {"IsPaid", type logical}
    }),
    AddedMonth = Table.AddColumn(TypedData, "Month", each Date.MonthName([AppointmentDate])),
    AddedPaymentStatus = Table.AddColumn(AddedMonth, "PaymentStatus", each if [IsPaid] then "Paid" else "Unpaid")
in
    AddedPaymentStatus
```

## ✅ Quick Setup Checklist

### Chuẩn bị dữ liệu (5 phút)

- [ ] API đang chạy tại localhost:8080
- [ ] Test API endpoint trả về JSON data
- [ ] Power BI Desktop đã cài đặt

### Tạo Data Model (10 phút)

- [ ] Load data từ API
- [ ] Transform data với Power Query
- [ ] Tạo các calculated columns cần thiết
- [ ] Validate data types

### Tạo 6 Charts (15 phút)

- [ ] Line Chart: Appointments Trend
- [ ] Bar Chart: Revenue by Service
- [ ] Pie Chart: Status Distribution
- [ ] Column Chart: Monthly Revenue
- [ ] Donut Chart: Payment Status
- [ ] Table: Detailed View

### Final Steps (5 phút)

- [ ] Add slicers cho filtering
- [ ] Format charts cho đẹp
- [ ] Test interactivity
- [ ] Save .pbix file

## 🔄 Troubleshooting & Tips

### Common Issues

1. **API không kết nối được**

   ```
   Solution:
   - Kiểm tra API đang chạy: http://localhost:8080/api/AppointmentsTienDM
   - Thử browser trước khi dùng Power BI
   ```

2. **Data không load được**

   ```
   Solution:
   - Kiểm tra JSON format từ API
   - Dùng Postman test API trước
   ```

3. **Date format lỗi**
   ```
   Solution:
   - Set đúng data type trong Power Query
   - Format: YYYY-MM-DD
   ```

## 📋 Sample API Response

```json
[
  {
    "appointmentsTienDmid": 1,
    "userAccountId": 101,
    "appointmentDate": "2025-01-15",
    "appointmentTime": "09:30:00",
    "totalAmount": 750000,
    "isPaid": true,
    "statusName": "Completed",
    "serviceName": "DNA Paternity Test",
    "username": "john.doe@email.com",
    "samplingMethod": "Buccal Swab"
  }
]
```

---

**Tổng thời gian setup:** ~35 phút  
**Kết quả:** Dashboard hoàn chỉnh với 6 charts cơ bản từ dữ liệu API
