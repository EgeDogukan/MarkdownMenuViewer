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
        console.log(data);
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };

    fetchData();
  }, []);

  // Dosyaya tıklandığında bu fonksiyonu çağır ve seçilen dosyayı güncelle
  const handleFileClick = (fileName) => {
    setSelectedFile(fileName);
  };

  return (
    <>
      <div className="container-m m-0" id="main-container">
        <div id="file-container">
          <div id="filess" className="d-flex justify-content  ">
            <a href=""><i id="arrow" className="bi bi-arrow-left-circle"></i></a>
            <h1 id="files-heading">Files</h1>
          </div>
          <div>
            <div id="input" className="input-group">
              <div id="placeholder" className="form-outline d-flex justify-content" data-mdb-input-init>
                <input id="search-input" type="search" placeholder="Search your document" className="form-control" />
                <button id="search-button" type="button" className="btn btn-primary">
                  <i className="bi bi-search"></i>
                </button>
              </div>
            </div>
          </div>

          {/* FileExplorer component */}
          {apiFiles && (
            <FileExplorer files={apiFiles} onFileClick={handleFileClick} />
          )}
        </div>
        <div id="content">
          <div id="md-content-one">
            <h2><i className="bi bi-filetype-md"></i> MarkdownMenuViewer</h2>
          </div>
          <div id="md-content-two">
            {/* Seçilen dosya varsa, bu bölümde göster */}
            {selectedFile && (
              <h5>{`"${selectedFile}" dosyası içeriği `}</h5>
            )}
          </div>
        </div>
      </div>
    </>
  );
}

// FileExplorer component
function FileExplorer({ files, onFileClick, }) {
  
  const [expanded, setExpanded] = useState(false);

  const toggleIcon = expanded ? "bi bi-chevron-down" : "bi bi-chevron-right";

  const handleToggle = () => {
    setExpanded(!expanded);
  };
  const Parsed = JSON.parse(JSON.stringify(files));
   
  if (Parsed.type === "folder") {
    return (
      <div id="con" key={Parsed.name}>
        <span id="spann" onClick={handleToggle}>
          <i className={toggleIcon}></i>📂&nbsp;{Parsed.name}
        </span>
        <div className="expanded" style={{ display: expanded ? "block" : "none" }}>
          {Parsed.data.map((file) => (
            file.type === "file" ? (
              <div
                id="expanded-content"
                key={file.name}
                onClick={() => onFileClick(file.name)}
              >
                &nbsp;&nbsp;&nbsp;&nbsp;<i className="bi bi-file-earmark"></i>&nbsp;{file.name}
              </div>
            ) : (
              <FileExplorer
                key={file.name}
                Parsed={file}
                onFileClick={onFileClick}
              />
            )
          ))}
        </div>
      </div>
    );
  } else if (Parsed.type === "file") {
    return <div>{Parsed.name}</div>;
  } else {
    return <p>!!</p>;
  }
}
