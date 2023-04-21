# PTUD-Web-NC

=========================Lab01==========================
C. Bài tập thực hành

=========================Lab02==========================
C. Bài tập thực hành

1. Hãy thay đổi hoặc bổ sung mã nguồn trong tập tin _PostItem.cshtml để hiển
thị tiêu đề bài viết, tên tác giả và tên chuyên mục dưới dạng liên kết

2. Cài đặt các action trong lớp BlogController và các view tương ứng.
 	 Category(string slug): để hiển thị danh sách bài viết thuộc chủ đề (do
	người dùng chọn trong phần sidebar hoặc trong mỗi bài viết).

	 Author(string slug): để hiển thị danh sách bài viết theo tác giả (do người
	dùng chọn trong liên kết ở dưới phần tiêu đề bài viết).

	 Tag(string slug): để hiển thị danh sách bài viết chứa thẻ (tag do người
	dùng chọn trong phần tag cloud ở sidebar hoặc trong mỗi bài viết).

	 Post(int year, int month, int day, string slug): để hiển thị chi tiết một bài
	viết khi người dùng nhấn vào nút Xem chi tiết hoặc tiêu đề bài viết ở trang
	chủ. Khi người dùng xem chi tiết một bài viết, tăng số lượt xem của bài viết
	đó lên 1 lượt. Nếu trạng thái IsPublished của bài viết là False thì hiển thị
	thông báo lỗi.

	 Archives(int year, int month): để hiển thị danh sách bài viết được đăng
	trong tháng và năm đã chọn (do người dùng click chuột vào các tháng
	trong view component Archives ở bài tập 3).

	 Contact(): để hiển thị thông tin liên hệ, bản đồ và form để gửi ý kiến.

	 About(): để hiển thị trang giới thiệu về blog (nội dung tĩnh).


3. Tạo các view component và thêm chúng vào sidebar:
	 FeaturedPosts: Hiển thị TOP 3 bài viết được xem nhiều nhất. Người dùng
	có thể click chuột để xem chi tiết.

	 RandomPosts: Hiển thị TOP 5 bài viết ngẫu nhiên. Người dùng có thể click
	chuột để xem chi tiết.

	 TagCloud: Hiển thị danh sách các thẻ (tag). Khi người dùng click chuột vào
	thẻ nào thì hiển thị danh sách bài viết chứa thẻ đó.


	 BestAuthors: Hiển thị TOP 4 tác giả có nhiều bài viết nhất. Sử dụng view
	component này trên trang chủ. Khi người dùng click chuột vào tên tác giả,
	hiển thị danh sách bài viết của tác giả đó.

	 Archives: Hiển thị danh sách 12 tháng gần nhất và số lượng bài viết trong
	mỗi tháng dưới dạng các liên kết. Khi người dùng click chuột vào tháng nào
	thì hiển thị danh sách bài viết được đăng trong tháng đó. Định dạng:
	November 2022 (5), February 2023 (11)

4. Xây dựng chức năng đăng ký nhận thông báo khi có bài viết mới
	 Tạo partial view NewsletterForm cho phép người dùng nhập địa chỉ email
	để đăng ký nhận thông báo khi có bài viết mới.

	 Tạo controller mới, đặt tên là NewsletterController.

|=========================Chưa hoàn thiện==========================
|	 Tạo 2 action Subscribe(string email), Unsubscribe(string email) để
|	thực hiện việc đăng ký và hủy đăng ký nhận thông báo bài viết mới.
|
|	 Khi người dùng nhấn nút submit (Subscribe), địa chỉ email được gửi tới
|	action Subscribe, sử dụng các phương thức đã cài đặt trong bài tập 3 ở
|	Lab 01 để lưu lại thông tin. Ngoài ra, hệ thống sẽ gửi 1 email tới địa chỉ của
|	người dùng, trong đó có nội dung cảm ơn vì đã đăng ký và một liên kết
|	(URL) tới action Ubsubscribe để người dùng có thể hủy đăng ký bất cứ lúc
|	nào.
|
==================Lab03===================
C. Bài tập thực hành
1. Tiếp tục hoàn thiện các chức năng quản lý bài viết.
	 Trong phần hướng dẫn, trang hiển thị danh sách bài viết luôn hiển thị 10
	bài viết mới nhất. Hãy cập nhật lại mã nguồn và thêm điều khiển phân
	trang để người quản trị có thể tải và xem tất cả các bài viết. (Sinh viên
	tham khảo cách xây dựng điều khiển phân trang ở bài Lab trước).

	 Hiện tại cột “Xuất bản” hiển thị một trong 2 giá trị: “Có”, “Không”. Hãy cập
	nhật lại mã nguồn để hiển thị giá trị này dưới dạng nút bấm. Khi người
	dùng click chuột thì đổi trạng thái Xuất bản của bài viết.

	 Bổ sung thêm mã lệnh để hiển thị nút xóa trên mỗi dòng ứng với bài viết.
	Khi người dùng click chuột vào nút này thì hỏi “Bạn có thực sự muốn xóa
	bài viết này không?”. Nếu người dùng trả lời Yes, thực hiện việc xóa bài viết
	và tải lại trang.

	 Trên khung tìm kiếm, bổ sung thêm điều kiện tìm kiếm “Chưa xuất bản”,
	hiển thị dưới dạng checkbox. Khi người dùng đánh dấu chọn checkbox này
	và nhấn tìm kiếm thì chỉ hiển thị những bài viết có cờ Published bằng false.

	 Trên khung tìm kiếm, thêm nút “Bỏ lọc”. Khi người dùng nhấn vào nút này
	thì xóa tất cả các điều kiện tìm kiếm trong các ô nhập và tải lại trang chứa
	đầy đủ bài viết.

2. Cài đặt các chức năng xem danh sách, thêm, xóa, cập nhật chủ đề.
3. Cài đặt các chức năng xem danh sách, thêm, xóa, cập nhật tác giả.
4. Cài đặt các chức năng xem danh sách, thêm, xóa, cập nhật thẻ (tag).

|=========================Chưa hoàn thiện==========================
|5. Cài đặt các chức năng xem danh sách, phê duyệt, xóa các bình luận.
|6. Cài đặt các chức năng xem danh sách, quản lý người đăng ký theo dõi blog.
|
|7. Ở trang “Bảng điều khiển” (action Index, controller Dashboard), hãy tạo các
|	thống kê về: Tổng số bài viết, số bài viết chưa xuất bản, số lượng chủ đề, số
|	lượng tác giả, số lượng bình luận đang chờ phê duyệt, số lượng người theo
|	dõi, số lượng người mới theo dõi đăng ký (lấy số liệu trong ngày).
|	(Ví dụ cách hiển thị các con số thống kê)Phát triển ứng dụng Web nâng cao 2023
|
|8. Tìm hiểu và áp dụng TinyMCE (hoặc thư viện tương tự) vào dự án để tạo
|	RichTextEditor. Áp dụng cho trường nhập nội dung của bài viết.
|
|9. Tìm hiểu cách sử dụng jqGrid (DataTables.js hoặc các thư viện tương tự) để
|	hiển thị danh sách bài viết (chủ đề, tác giả, …) dưới dạng bảng, hỗ trợ phân
|	trang, sắp xếp các mẫu tin và tải dữ liệu bằng AJAX.
|
|==============================Lab 04================================

1. Xây dựng các API endpoint categories để quản lý thông tin các chuyên mục.

2. Xây dựng các API endpoint posts để quản lý thông tin bài viết.

3. Xây dựng các API endpoint để quản lý thẻ/từ khóa (tag).

4. Xây dựng các API endpoint để quản lý thông tin người theo dõi blog, đăng ký
và hủy đăng ký theo dõi.

5. Xây dựng các API endpoint để quản lý các bình luận.

6. Xây dựng các API endpoint để xử lý việc nhận các góp ý từ người dùng (nhập
vào form liên hệ).

7. Xây dựng các API endpoint trả về các chỉ số thống kê: Tổng số bài viết, số bài
viết chưa xuất bản, số lượng chủ đề, số lượng tác giả, số lượng bình luận đang
chờ phê duyệt, số lượng người theo dõi, số lượng người mới theo dõi đăng ký
(lấy số liệu trong ngày).

8. Tìm hiểu cách sử dụng gói thư viện Carter để phân chia API endpoints vào các
mô-đun và tự động tìm quét các endpoint.
|==============================Lab 05================================
|==============================Lab 06================================
|==============================Lab Tablog============================
