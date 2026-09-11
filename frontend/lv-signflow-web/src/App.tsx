import { useEffect, useRef, useState } from "react";
import { Document, Page, pdfjs } from "react-pdf";
import { Rnd } from "react-rnd";
import {
  Box,
  Button,
  Checkbox,
  FormControl,
  FormControlLabel,
  InputLabel,
  MenuItem,
  Paper,
  Select,
  Typography,
  TextField,
} from "@mui/material";

pdfjs.GlobalWorkerOptions.workerSrc = new URL(
  "pdfjs-dist/build/pdf.worker.min.mjs",
  import.meta.url,
).toString();

type FieldType = "signature" | "date" | "text";

type RecipientRole = {
  id: string;
  name: string;
};
type PdfField = {
  id: string;
  type: FieldType;

  recipientRoleId: string;

  page: number;

  x: number;
  y: number;
  width: number;
  height: number;
  isRequired: boolean;
};
type TemplateDraft = {
  id: string;
  name: string;

  recipientRoles: RecipientRole[];

  fields: PdfField[];

  createdAt: string;
  updatedAt: string;
};

const recipientRoles: RecipientRole[] = [
  {
    id: "employee",
    name: "Employee",
  },
  {
    id: "manager",
    name: "Manager",
  },
  {
    id: "director",
    name: "Director",
  },
];
function App() {
  const [numPages, setNumPages] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);
const [templateName, setTemplateName] =
  useState<string>("Business Travel Template");
  const pageContainerRef = useRef<HTMLDivElement | null>(null);

  const [pageSize, setPageSize] = useState({
    width: 800,
    height: 1000,
  });

  const [fields, setFields] = useState<PdfField[]>([]);
  const [selectedRecipientRoleId, setSelectedRecipientRoleId] =
    useState<string>("employee");
  const [selectedFieldId, setSelectedFieldId] = useState<string | null>(null);
  const selectedField =
    fields.find((field) => field.id === selectedFieldId) ?? null;

  useEffect(() => {
    const container = pageContainerRef.current;

    if (!container) return;

    const updateSize = () => {
      setPageSize({
        width: container.clientWidth,
        height: container.clientHeight,
      });
    };

    updateSize();

    const observer = new ResizeObserver(updateSize);

    observer.observe(container);

    return () => {
      observer.disconnect();
    };
  }, []);

  const handleFieldDragStart = (
    event: React.DragEvent<HTMLDivElement>,
    fieldType: FieldType,
  ) => {
    event.dataTransfer.setData("fieldType", fieldType);
  };

  const handleDrop = (event: React.DragEvent<HTMLDivElement>) => {
    event.preventDefault();

    const container = pageContainerRef.current;

    if (!container) return;

    const fieldType = event.dataTransfer.getData("fieldType") as FieldType;

    if (!fieldType) return;

    const rect = container.getBoundingClientRect();

    const dropX = event.clientX - rect.left;
    const dropY = event.clientY - rect.top;

    const defaultWidth = 0.2;
    const defaultHeight = 0.06;

    const normalizedX = dropX / rect.width;
    const normalizedY = dropY / rect.height;

    const newField: PdfField = {
      id: crypto.randomUUID(),
      type: fieldType,

      recipientRoleId: selectedRecipientRoleId,

      page: currentPage,

      x: Math.min(Math.max(normalizedX, 0), 1 - defaultWidth),
      y: Math.min(Math.max(normalizedY, 0), 1 - defaultHeight),

      width: defaultWidth,
      height: defaultHeight,
      isRequired: true,
    };

    setFields((previous) => [...previous, newField]);

    setSelectedFieldId(newField.id);
  };

  const updateField = (fieldId: string, changes: Partial<PdfField>) => {
    setFields((previous) =>
      previous.map((field) =>
        field.id === fieldId
          ? {
              ...field,
              ...changes,
            }
          : field,
      ),
    );
  };
  const deleteField = (fieldId: string) => {
    setFields((previous) => previous.filter((field) => field.id !== fieldId));

    setSelectedFieldId(null);
  };

  const updateFieldPosition = (fieldId: string, x: number, y: number) => {
    const container = pageContainerRef.current;

    if (!container) return;

    setFields((previous) =>
      previous.map((field) =>
        field.id === fieldId
          ? {
              ...field,
              x: x / container.clientWidth,
              y: y / container.clientHeight,
            }
          : field,
      ),
    );
  };

  const updateFieldSize = (
    fieldId: string,
    x: number,
    y: number,
    width: number,
    height: number,
  ) => {
    const container = pageContainerRef.current;

    if (!container) return;

    setFields((previous) =>
      previous.map((field) =>
        field.id === fieldId
          ? {
              ...field,
              x: x / container.clientWidth,
              y: y / container.clientHeight,
              width: width / container.clientWidth,
              height: height / container.clientHeight,
            }
          : field,
      ),
    );
  };

  const getFieldLabel = (type: FieldType) => {
    switch (type) {
      case "signature":
        return "Signature";

      case "date":
        return "Date";

      case "text":
        return "Text";

      default:
        return type;
    }
  };
  const getRecipientName = (recipientRoleId: string) => {
    return (
      recipientRoles.find((role) => role.id === recipientRoleId)?.name ??
      "Unknown"
    );
  };

  const saveTemplate = () => {
  const now = new Date().toISOString();

  const template: TemplateDraft = {
    id: "template-poc-001",
    name: templateName,

    recipientRoles,
    fields,

    createdAt: now,
    updatedAt: now,
  };

  localStorage.setItem(
    "lv-signflow-template-poc",
    JSON.stringify(template)
  );

  console.log("Template saved:", template);

  alert("Template saved.");
};

const loadTemplate = () => {
  const storedTemplate =
    localStorage.getItem(
      "lv-signflow-template-poc"
    );

  if (!storedTemplate) {
    alert("Saved template tapılmadı.");
    return;
  }

  const template: TemplateDraft =
    JSON.parse(storedTemplate);

  setTemplateName(template.name);
  setFields(template.fields);

  setSelectedFieldId(null);

  console.log(
    "Template loaded:",
    template
  );
};

const clearTemplateFields = () => {
  setFields([]);
  setSelectedFieldId(null);
};


  return (
    <Box
      sx={{
        minHeight: "100vh",
        backgroundColor: "#f5f5f5",
        p: 4,
      }}
    >
      <Typography variant="h4" gutterBottom>
        LV SignFlow
      </Typography>
<TextField
  label="Template Name"
  value={templateName}
  onChange={(event) =>
    setTemplateName(event.target.value)
  }
  sx={{
    mb: 3,
    width: 400,
  }}
/><Box
  sx={{
    display: "flex",
    gap: 1,
    mb: 3,
  }}
>
  <Button
    variant="contained"
    onClick={saveTemplate}
  >
    Save Template
  </Button>

  <Button
    variant="outlined"
    onClick={loadTemplate}
  >
    Load Template
  </Button>

  <Button
    variant="outlined"
    color="warning"
    onClick={clearTemplateFields}
  >
    Clear Fields
  </Button>
</Box>
      <Typography variant="body1" sx={{ mb: 3 }}>
        PDF Template Builder — Proof of Concept
      </Typography>

      <Box
        sx={{
          display: "flex",
          gap: 3,
          alignItems: "flex-start",
        }}
      >
        {/* LEFT FIELD PANEL */}

        <Paper
          sx={{
            width: 240,
            p: 2,
          }}
        >
          <Typography variant="h6" sx={{ mb: 2 }}>
            Recipients
          </Typography>

          <Box
            sx={{
              display: "flex",
              flexDirection: "column",
              gap: 1,
              mb: 3,
            }}
          >
            {recipientRoles.map((role) => (
              <Button
                key={role.id}
                variant={
                  selectedRecipientRoleId === role.id ? "contained" : "outlined"
                }
                onClick={() => setSelectedRecipientRoleId(role.id)}
                sx={{
                  justifyContent: "flex-start",
                  textTransform: "none",
                }}
              >
                {role.name}
              </Button>
            ))}
          </Box>
          <Typography variant="h6" sx={{ mb: 2 }}>
            Fields
          </Typography>

          <Box
            draggable
            onDragStart={(event) => handleFieldDragStart(event, "signature")}
            sx={{
              p: 1.5,
              mb: 1,
              border: "1px solid #ccc",
              borderRadius: 1,
              cursor: "grab",
              backgroundColor: "#fff",
            }}
          >
            Signature
          </Box>

          <Box
            draggable
            onDragStart={(event) => handleFieldDragStart(event, "date")}
            sx={{
              p: 1.5,
              mb: 1,
              border: "1px solid #ccc",
              borderRadius: 1,
              cursor: "grab",
              backgroundColor: "#fff",
            }}
          >
            Date
          </Box>

          <Box
            draggable
            onDragStart={(event) => handleFieldDragStart(event, "text")}
            sx={{
              p: 1.5,
              border: "1px solid #ccc",
              borderRadius: 1,
              cursor: "grab",
              backgroundColor: "#fff",
            }}
          >
            Text
          </Box>
        </Paper>

        {/* PDF */}

        <Paper
          elevation={2}
          sx={{
            p: 3,
            width: "fit-content",
          }}
        >
          <Document
            file="/sample.pdf"
            onLoadSuccess={({ numPages }) => {
              setNumPages(numPages);
            }}
            loading={<Typography>PDF yüklənir...</Typography>}
            error={<Typography color="error">PDF yüklənə bilmədi.</Typography>}
          >
            <Box
              ref={pageContainerRef}
              onDragOver={(event) => event.preventDefault()}
              onDrop={handleDrop}
              sx={{
                position: "relative",
                width: 800,
              }}
            >
              <Page
                pageNumber={currentPage}
                width={800}
                renderTextLayer={false}
                renderAnnotationLayer={false}
              />

              {fields
                .filter((field) => field.page === currentPage)
                .map((field) => (
                  <Rnd
                    key={field.id}
                    bounds="parent"
                    onMouseDown={() => {
                      setSelectedFieldId(field.id);
                    }}
                    size={{
                      width: field.width * pageSize.width,
                      height: field.height * pageSize.height,
                    }}
                    position={{
                      x: field.x * pageSize.width,
                      y: field.y * pageSize.height,
                    }}
                    onDragStop={(_, data) => {
                      updateFieldPosition(field.id, data.x, data.y);
                    }}
                    onResizeStop={(_, __, ref, ___, position) => {
                      updateFieldSize(
                        field.id,
                        position.x,
                        position.y,
                        ref.offsetWidth,
                        ref.offsetHeight,
                      );
                    }}
                    style={{
                      border:
                        selectedFieldId === field.id
                          ? "2px solid #1976d2"
                          : "2px dashed #1976d2",
                      background: "rgba(25,118,210,0.12)",
                      display: "flex",
                      alignItems: "center",
                      justifyContent: "center",
                      cursor: "move",
                      zIndex: 10,
                    }}
                  >
                    <Box
                      sx={{
                        textAlign: "center",
                        userSelect: "none",
                      }}
                    >
                      <Typography
                        sx={{
                          fontSize: 13,
                          fontWeight: 600,
                          lineHeight: 1.2,
                        }}
                      >
                      {getFieldLabel(field.type)}
{field.isRequired ? " *" : ""}
                      </Typography>

                      <Typography
                        sx={{
                          fontSize: 10,
                          opacity: 0.7,
                          lineHeight: 1.2,
                        }}
                      >
                        {getRecipientName(field.recipientRoleId)}
                      </Typography>
                    </Box>
                  </Rnd>
                ))}
            </Box>
          </Document>

          {/* PAGE CONTROLS */}

          <Box
            sx={{
              mt: 2,
              display: "flex",
              justifyContent: "center",
              alignItems: "center",
              gap: 2,
            }}
          >
            <Button
              variant="outlined"
              disabled={currentPage <= 1}
              onClick={() => setCurrentPage((page) => page - 1)}
            >
              Previous
            </Button>

            <Typography>
              Page {currentPage} / {numPages}
            </Typography>

            <Button
              variant="outlined"
              disabled={currentPage >= numPages}
              onClick={() => setCurrentPage((page) => page + 1)}
            >
              Next
            </Button>
          </Box>

          {/* DEBUG DATA */}

          <Box
            sx={{
              mt: 3,
              p: 2,
              backgroundColor: "#eee",
              maxWidth: 800,
              overflow: "auto",
            }}
          >
            <Typography variant="subtitle2" sx={{ mb: 1 }}>
              Template Fields
            </Typography>

            <Typography
              component="pre"
              sx={{
                fontSize: 12,
              }}
            >
              {JSON.stringify(fields, null, 2)}
            </Typography>
          </Box>
        </Paper>

        <Paper
  sx={{
    width: 280,
    p: 2,
    position: "sticky",
    top: 20,
  }}
>
  <Typography
    variant="h6"
    sx={{ mb: 2 }}
  >
    Properties
  </Typography>

  {!selectedField ? (
    <Typography
      variant="body2"
      color="text.secondary"
    >
      Select a field from the PDF.
    </Typography>
  ) : (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        gap: 2,
      }}
    >
      <Typography
        variant="caption"
        color="text.secondary"
      >
        Page {selectedField.page}
      </Typography>

      <FormControl fullWidth size="small">
        <InputLabel>
          Field Type
        </InputLabel>

        <Select
          value={selectedField.type}
          label="Field Type"
          onChange={(event) => {
            updateField(
              selectedField.id,
              {
                type: event.target
                  .value as FieldType,
              }
            );
          }}
        >
          <MenuItem value="signature">
            Signature
          </MenuItem>

          <MenuItem value="date">
            Date
          </MenuItem>

          <MenuItem value="text">
            Text
          </MenuItem>
        </Select>
      </FormControl>

      <FormControl fullWidth size="small">
        <InputLabel>
          Recipient
        </InputLabel>

        <Select
          value={
            selectedField.recipientRoleId
          }
          label="Recipient"
          onChange={(event) => {
            updateField(
              selectedField.id,
              {
                recipientRoleId:
                  event.target.value,
              }
            );
          }}
        >
          {recipientRoles.map(
            (role) => (
              <MenuItem
                key={role.id}
                value={role.id}
              >
                {role.name}
              </MenuItem>
            )
          )}
        </Select>
      </FormControl>

      <FormControlLabel
        control={
          <Checkbox
            checked={
              selectedField.isRequired
            }
            onChange={(event) => {
              updateField(
                selectedField.id,
                {
                  isRequired:
                    event.target.checked,
                }
              );
            }}
          />
        }
        label="Required"
      />

      <Box
        sx={{
          backgroundColor: "#f5f5f5",
          p: 1.5,
          borderRadius: 1,
        }}
      >
        <Typography
          variant="caption"
          component="div"
        >
          Position
        </Typography>

        <Typography
          variant="body2"
          component="div"
        >
          X: {selectedField.x.toFixed(3)}
        </Typography>

        <Typography
          variant="body2"
          component="div"
        >
          Y: {selectedField.y.toFixed(3)}
        </Typography>

        <Typography
          variant="body2"
          component="div"
        >
          Width:{" "}
          {selectedField.width.toFixed(3)}
        </Typography>

        <Typography
          variant="body2"
          component="div"
        >
          Height:{" "}
          {selectedField.height.toFixed(3)}
        </Typography>
      </Box>

      <Button
        variant="outlined"
        color="error"
        onClick={() =>
          deleteField(selectedField.id)
        }
      >
        Delete Field
      </Button>
    </Box>
  )}
</Paper>
      </Box>
    </Box>
  );
}

export default App;
