INSERT INTO Departments (DepartmentName) VALUES 
('Bilgi Ýþlem'), ('Ýnsan Kaynaklarý'), ('Muhasebe'), ('Satýþ')
GO

INSERT INTO Priorities (PriorityName, ColorCode) VALUES 
('Düþük', '#28a745'), ('Normal', '#ffc107'), ('Yüksek', '#fd7e14'), ('Acil', '#dc3545')
GO

INSERT INTO Statuses (StatusName, ColorCode) VALUES 
('Açýk', '#007bff'), ('Devam Ediyor', '#ffc107'), ('Beklemede', '#6c757d'), ('Çözüldü', '#28a745'), ('Kapatýldý', '#dc3545')
GO

INSERT INTO Employees (FirstName, LastName, Email, Phone, DepartmentId) VALUES 
('Ahmet', 'Yýlmaz', 'ahmet@firma.com', '555-0001', 1),
('Ayþe', 'Kaya', 'ayse@firma.com', '555-0002', 2),
('Mehmet', 'Demir', 'mehmet@firma.com', '555-0003', 1),
('Fatma', 'Çelik', 'fatma@firma.com', '555-0004', 3)
GO

INSERT INTO Tickets (Title, Description, RequesterId, AssignedToId, PriorityId, StatusId) VALUES 
('Bilgisayar açýlmýyor', 'Bilgisayarým açýlmýyor, yardým lütfen', 2, 1, 3, 1),
('Email eriþim sorunu', 'Email hesabýma giremiyorum', 4, 3, 2, 2),
('Yazýcý çalýþmýyor', 'Yazýcýdan çýktý alamýyorum', 2, 1, 1, 4)
GO