using System;
using System.Collections.Generic;
using System.Text;

namespace HortiBts.Shared.Dtos.Notices
{
    // Maps to noticeboard_file_path.
    // GET /api/notices/{noticeId}/documents
    public class NoticeDocDto
    {
        public int? Id { get; set; }
        public int? NoticeId { get; set; }
        public string FileName { get; set; } = "";   // human-readable label, for display
        public string Path { get; set; } = "";       // actual filename on disk, used for download
    }

    public class NoticesDto
    {
        // Maps to noticeboard LEFT JOIN noticeboard_file_path.
        // GET /api/notices

        public int NoticeId { get; set; }
        public string Subject { get; set; } = "";
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Priority { get; set; }               // "H" / "M" / "L" etc.
        public string NoticeType { get; set; } = "";        // GN / EV / AN / AL / NW
        public bool? Status { get; set; }        
        public string? FileName { get; set; }
        public string? Path { get; set; }
    }
}
