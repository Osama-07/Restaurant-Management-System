# مرحلة البناء
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# نسخ ملفات الحلول والمشاريع
COPY ["RestaurantAPI.sln", "."]
COPY ["Restaurant_Data_Access/Restaurant_Data_Access.csproj", "Restaurant_Data_Access/"]
COPY ["Restaurant_Business/Restaurant_Business.csproj", "Restaurant_Business/"]
COPY ["RestaurantAPI/RestaurantAPI.csproj", "RestaurantAPI/"]

# استعادة الحزم
RUN dotnet restore "RestaurantAPI/RestaurantAPI.csproj"

# نسخ باقي الملفات
COPY . .

# نسخ مكتبة Util.dll إلى المسار المطلوب داخل الحاوية
COPY ["D:/سطح المكتب/Osama Projects/Util/bin/Debug/Util.dll", "/src/Restaurant_Data_Access/"]

# بناء المشروع
RUN dotnet build "RestaurantAPI/RestaurantAPI.csproj" -c Release -o /app/build

# مرحلة النشر
FROM build AS publish
RUN dotnet publish "RestaurantAPI/RestaurantAPI.csproj" -c Release -o /app/publish

# مرحلة التشغيل
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestaurantAPI.dll"]
