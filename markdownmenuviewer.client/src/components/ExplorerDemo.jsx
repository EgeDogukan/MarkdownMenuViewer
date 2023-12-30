/* eslint-disable react/prop-types */
import { useState, useEffect } from "react";
import "../CSS/style.css";

export default function Explore() {
  const [selectedFile, setSelectedFile] = useState(null);
  const [apiFiles, setApiFiles] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await fetch(
          "https://658e9ba72871a9866e7973f5.mockapi.io/markdown/v1/folder"
        );
        const data = await response.json();
        setApiFiles(data);
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };

    fetchData();
  }, []);

  const handleFileClick = (fileName) => {
    setSelectedFile(fileName);
  };

  return (
    <>
      <div className="container-m m-0" id="main-container">
        <div id="file-container">
          {/* ... (existing code) */}
          <FileExplorer files={apiFiles} onFileClick={handleFileClick} />
        </div>

        <div id="content">
          <div id="md-content-one">
            <h2><i className="bi bi-filetype-md"></i> MarkdownMenuViewer</h2>
          </div>

          <div id="md-content-two">
            {selectedFile && (
              <h5>{`"${selectedFile}" dosyası içeriği `}</h5>
            )}
          </div>
        </div>
      </div>
    </>
  );
}

// ... (existing code)

// FileExplorer component
function FileExplorer({ files, onFileClick }) {
  const [expanded, setExpanded] = useState(false);

  const toggleIcon = expanded ? "bi bi-chevron-down" : "bi bi-chevron-right";

  const handleToggle = () => {
    setExpanded(!expanded);
  };

  if (!files) {
    return null; // Handle the case when data is still loading
  }

  if (files.type === "folder") {
    return (
      <div id="con" key={files.name}>
        {/* ... (existing code) */}
      </div>
    );
  } else if (files.type === "file") {
    return (
      <div id="expanded-content" onClick={() => onFileClick(files.name)}>
        &nbsp;&nbsp;&nbsp;&nbsp;<i className="bi bi-file-earmark"></i>&nbsp;{files.name}
      </div>
    );
  } else {
    return <p>!!</p>;
  }
}
