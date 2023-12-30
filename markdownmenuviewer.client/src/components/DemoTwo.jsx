/* eslint-disable react/prop-types */
import { useState } from "react";
import "../CSS/style.css";

export default function Explore() {
  const [selectedFile, setSelectedFile] = useState(null);

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

          <FileExplorer files={Files} onFileClick={handleFileClick} />
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

let Files = {


  type: "folder",
  name: "Ana Dizin",
  data: [

    {
      type: "folder",
      name: "MarkdownMenuViewer.Server",
      data: [
        {
          type: "folder",
          name: "src",
          data: [
            {
              type: "file",
              name: "index.js"
            }
          ]
        },
        {
          type: "folder",
          name: "public",
          data: [
            {
              type: "file",
              name: "index.ts"
            }
          ]
        },
        {
          type: "file",
          name: "index.html"
        },
        {
          type: "folder",
          name: "data",
          data: [
            {
              type: "folder",
              name: "images",
              data: [
                {
                  type: "file",
                  name: "image.jpg"
                },
                {
                  type: "file",
                  name: "image2.jpg"
                },
                {
                  type: "file",
                  name: "image3.jpg"
                }
              ]
            },
            {
              type: "file",
              name: "logo.svg"
            }
          ]
        },
        {
          type: "file",
          name: "style.css"
        }
      ]
    },
    {
      type: "folder",
      name: "Markdownmenuviewer.client",
      data: [
        {
          type: "file",
          name: "typscript-1.ts"
        },
        {
          type: "file",
          name: "typscript-2.ts"
        },
        {
          type: "file",
          name: "typscrip3.ts"
        }
      ]
    },

    {
      type: "file",
      name: ".gitattributes"
    },
    {
      type: "file",
      name: ".gitignore"
    },
    {
      type: "file",
      name: "MarkdownMenuViewer.sln"
    }
  ]



};

function FileExplorer({ files, onFileClick }) {
  const [expanded, setExpanded] = useState(false);

  const toggleIcon = expanded ? "bi bi-chevron-down" : "bi bi-chevron-right";

  const handleToggle = () => {
    setExpanded(!expanded);
  };

  if (files.type === "folder") {
    return (
      <div id="con" key={files.name}>
        <span id="spann" onClick={handleToggle}>
          <i className={toggleIcon}></i>📂&nbsp;{files.name}
        </span>
        <div className="expanded"
          style={{ display: expanded ? "block" : "none" }}
        >
          {files.data.map((file) => {
            if (file.type === "file")
              return (
                // Dosyaya tıklandığında onFileClick fonksiyonunu çağır
                <div
                  id="expanded-content"
                  key={file.name}
                  onClick={() => onFileClick(file.name)}
                >
                  &nbsp;&nbsp;&nbsp;&nbsp;<i className="bi bi-file-earmark"></i>&nbsp;{file.name}
                </div>
              );
            else if (file.type === "folder")
              return (
                <FileExplorer
                  key={file.name}
                  files={file}
                  onFileClick={onFileClick}
                />
              );
          })}
        </div>
      </div>
    );
  } else if (files.type === "file") {
    return <div>{files.name}</div>;
  } else {
    return <p>!!</p>;
  }
}
