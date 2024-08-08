# Task - Net

## Question 1
1.  Jika di dalam suatu Gudang Super Market terdapat Pegawai A yang ingin melakukan pengecekan barang yang sudah kadaluarsa,
    berikan contoh solusi cara menanggulangi dan mengatisipasi dalam pengecekan barang yang sudah kadaluarsa.

Answer : 
    
    Solusi yang dapat dilakukan adalah dengan cara :
    
    - **Data Collection**
        -> Memastikan bahwa semua product/item memiliki tanggal expired yang sudah terdata pada sistem.

    - **Regular Check** 
        -> Melakukan pengecekan secara berkala untuk mengecek tanggal expired seperti membuat script untuk melakukan identifikasi barang mana yang akan expired dalam 30 hari ke depan.
        
        Algoritma :
            1. Mulai
            2. Akses database atau sistem penyimpanan data barang
            3. Buat query untuk mengambil data barang dengan kriteria:
                a. Tanggal expired kurang dari atau sama dengan 30 hari dari hari ini
                b. Status barang masih aktif (belum terjual/terpakai)
            4. Lakukan looping untuk setiap data barang yang memenuhi kriteria:
                a. Ekstrak informasi barang seperti:

                    i. Kode barang
                    
                    ii. Nama barang

                    iii. Tanggal expired

                    iv. Jumlah stok
            
                b. Simpan informasi barang ke dalam suatu struktur data (array, list, dll)
            5. Buat laporan atau notifikasi yang berisi daftar barang yang akan expired dalam 30 hari, beserta informasi terkait.
            6. Kirimkan laporan atau notifikasi tersebut kepada pihak yang bertanggung jawab (misalnya manajer gudang, tim logistik, dll)
            7. Selesai

    - **Notification**
        -> Memberikan alert atau notifikasi untuk memberitahu pegawai apabila ada product/item yang akan expired dalam 30 hari ke depan.

    - **Auto-Remove**
        -> Mengimplementasi sistem untuk menandai sebuah barang yang sudah kadaluarsa dan akan dihapus dari database.

## Question 2
- Buat 2 table :
        
        - Table Gudang
        - Table Barang

   berikan foreign key dan index.

- Buat store Prosedur tampilkan list data Kode Gudang, Nama Gudang, Kode Barang, Nama Barang, Harga Barang, Jumlah Barang, Expire Barang menggunakan Dynamic Query dan Paging.

- Buat Trigger ketika input barang di salah satu gudang munculkan barang yang kadaluarsa.

    - Answer :

    - Table Gudang & Barang
    ```sql
        CREATE TABLE Table_Gudang (
	        id SERIAL PRIMARY KEY,
            kode_gudang VARCHAR(100) NOT NULL,
            nama_gudang VARCHAR(100) NOT NULL
        );
    

    
        CREATE TABLE Table_Barang (
            id SERIAL PRIMARY KEY,
            kode_barang VARCHAR(50) NOT NULL,
            nama_barang VARCHAR(100) NOT NULL,
            harga_barang DECIMAL(10, 2) NOT NULL,
            jumlah_barang INT NOT NULL,
            expired_date DATE NOT NULL,
            gudang_id INT,
            FOREIGN KEY (gudang_id) REFERENCES Table_Gudang(id)
        );

        CREATE INDEX idx_expired_date ON Table_Barang(expired_date);
    ```

    
    - Dynamic Query & Paging

    ```sql
        CREATE OR REPLACE FUNCTION get_barang_list(
            page_limit INT,
        page_offset INT
        ) RETURNS TABLE(
            kode_gudang VARCHAR,
        nama_gudang VARCHAR,
        kode_barang VARCHAR,
        nama_barang VARCHAR,
        harga_barang DECIMAL,
        jumlah_barang INT,
        expired_date DATE
        ) AS $$

        BEGIN
            RETURN QUERY
            SELECT		g.kode_gudang,
                    g.nama_gudang,
                    b.kode_barang,
                    b.nama_barang,
                    b.harga_barang,
                    b.jumlah_barang,
                    b.expired_date
            FROM			Table_Barang b
            JOIN			Table_Gudang g ON b.gudang_id = g.id
            ORDER BY	g.nama_gudang, b.expired_date
            LIMIT			page_limit OFFSET page_offset;
        END;
        
        $$ LANGUAGE plpgsql;


        SELECT * FROM get_barang_list(3, 0);
    ```

    - Trigger
    ```sql
        CREATE OR REPLACE FUNCTION check_if_expired() RETURNS TRIGGER AS $$

        BEGIN
            IF NEW.expired_date < CURRENT_DATE THEN
            RAISE EXCEPTION 'Barang % di gudang % sudah kadaluarsa.', NEW.nama_barang, NEW.gudang_id;
        END IF;
        RETURN NEW;
        END;

        $$ LANGUAGE plpgsql;

        CREATE TRIGGER expired_item
        BEFORE INSERT OR UPDATE ON Table_Barang
        FOR EACH ROW EXECUTE FUNCTION check_if_expired();
    ```

## Question 3 : API Documentation

### Barang

#### GET
```json
GET {{marketAPI_HostAddress}}/api/Barang
Accept: application/json

Response:
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Thu, 08 Aug 2024 14:24:51 GMT
Server: Kestrel
Transfer-Encoding: chunked

[
  {
    "id": 1,
    "kodeBarang": "B001",
    "namaBarang": "Mie Goreng",
    "hargaBarang": 3000,
    "jumlahBarang": 50,
    "expiredDate": "2024-10-10T00:00:00Z",
    "gudangId": 1,
    "gudang": {
      "id": 1,
      "kodeGudang": "G01",
      "namaGudang": "GudangBesar01"
    }
  }
]
```

#### GET BY Nama Gudang
```json
GET {{marketAPI_HostAddress}}/api/Barang/monitoring?namaGudang=GudangBesar01
Accept: application/json

Response:
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Thu, 08 Aug 2024 14:33:58 GMT
Server: Kestrel
Transfer-Encoding: chunked

[
  {
    "id": 1,
    "kodeBarang": "B001",
    "namaBarang": "Mie Goreng",
    "hargaBarang": 3000,
    "jumlahBarang": 50,
    "expiredDate": "2024-10-10T00:00:00Z",
    "gudangId": 1,
    "gudang": {
      "id": 1,
      "kodeGudang": "G01",
      "namaGudang": "GudangBesar01"
    }
  },
  {
    "id": 2,
    "kodeBarang": "B002",
    "namaBarang": "Telur",
    "hargaBarang": 5000,
    "jumlahBarang": 50,
    "expiredDate": "2024-08-01T00:00:00Z",
    "gudangId": 1,
    "gudang": {
      "id": 1,
      "kodeGudang": "G01",
      "namaGudang": "GudangBesar01"
    }
  }
]
```

#### GET BY expired date
```json
GET {{marketAPI_HostAddress}}/api/Barang/monitoring?namaGudang=GudanBesar01&expiredDate=2024-10-10
Accept: application/json
```

#### POST
```json
POST {{marketAPI_HostAddress}}/api/Barang
Content-Type: application/json

Response:
HTTP/1.1 201 Created
Connection: close
Content-Type: application/json; charset=utf-8
Date: Thu, 08 Aug 2024 14:24:00 GMT
Server: Kestrel
Location: http://localhost:5126/api/Barang/1
Transfer-Encoding: chunked

{
  "id": 1,
  "kodeBarang": "B001",
  "namaBarang": "Mie Goreng",
  "hargaBarang": 3000,
  "jumlahBarang": 50,
  "expiredDate": "2024-10-10T00:00:00Z",
  "gudangId": 1,
  "gudang": null
}
```

#### PUT
```json
PUT {{marketAPI_HostAddress}}/api/Barang/3
Content-Type: application/json

Response:
HTTP/1.1 204 No Content
Connection: close
Date: Thu, 08 Aug 2024 14:27:30 GMT
Server: Kestrel
```

#### DELETE
```json
DELETE {{marketAPI_HostAddress}}/api/Barang/3

Response:
HTTP/1.1 200 OK
Connection: close
Content-Type: text/plain; charset=utf-8
Date: Thu, 08 Aug 2024 14:28:23 GMT
Server: Kestrel
Transfer-Encoding: chunked

Barang deleted successfully
```


### Gudang

#### GET
```json
GET {{marketAPI_HostAddress}}/api/Gudang
Accept: application/json

Response:
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Thu, 08 Aug 2024 14:03:44 GMT
Server: Kestrel
Transfer-Encoding: chunked

[
  {
    "id": 1,
    "kodeGudang": "G01",
    "namaGudang": "Gudang01"
  }
]
```

#### POST
```json
POST {{marketAPI_HostAddress}}/api/Gudang
Content-Type: application/json

{
    "Id": "1",
    "KodeGudang": "G01",
    "NamaGudang": "Gudang01"
}

Response:
HTTP/1.1 201 Created
Connection: close
Content-Type: application/json; charset=utf-8
Date: Thu, 08 Aug 2024 14:02:13 GMT
Server: Kestrel
Location: http://localhost:5126/api/Gudang/1
Transfer-Encoding: chunked

{
  "id": 1,
  "kodeGudang": "G01",
  "namaGudang": "Gudang01"
}
```

#### PUT
```json
PUT {{marketAPI_HostAddress}}/api/Gudang/1
Content-Type: application/json

{
    "Id": 1,
    "KodeGudang": "G01",
    "NamaGudang": "GudangBesar01"
}

Response:
HTTP/1.1 204 No Content
Connection: close
Content-Type: application/json; charset=utf-8
Date: Thu, 08 Aug 2024 14:02:13 GMT
Server: Kestrel
Transfer-Encoding: chunked

[
  {
    "id": 1,
    "kodeGudang": "G01",
    "namaGudang": "GudangBesar01"
  }
]
```

#### DELETE
```json
DELETE {{marketAPI_HostAddress}}/api/Gudang/3

Response:
HTTP/1.1 200 OK
Connection: close
Content-Type: text/plain; charset=utf-8
Date: Thu, 08 Aug 2024 14:08:48 GMT
Server: Kestrel
Transfer-Encoding: chunked

Gudang deleted successfully
```