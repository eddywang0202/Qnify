import { ITestQuestionResponse } from "~/common/interfaces/apis/question";

const data: ITestQuestionResponse = {
  "tsid": 11,
  "tst": "Head cancer",
  "tsq": [
    {
      "tqid": 44,
      "qid": 15,
      "qt": "Normal/Abnormal?",
      "qtid": 1,
      "cid": 0,
      "qgid": 2,
      "qpid": 0,
      "qo": 1,
      "a": [
        {
          "aid": 31,
          "at": "Abnormal",
          "nqid": 0,
          "ao": 1,
          "ica": true
        },
        {
          "aid": 32,
          "at": "Normal",
          "nqid": 0,
          "ao": 2,
          "ica": false
        }
      ]
    },
    {
      "tqid": 45,
      "qid": 16,
      "qt": "How confident are you about your answer?",
      "qtid": 5,
      "cid": 0,
      "qgid": 2,
      "qpid": 0,
      "qo": 2,
      "a": [
        {
          "aid": 41,
          "at": "Really Confident",
          "nqid": 0,
          "ao": 1,
          "ica": false
        },
        {
          "aid": 42,
          "at": "Minor skin folds",
          "nqid": 0,
          "ao": 1,
          "ica": false
        },
        {
          "aid": 40,
          "at": "Confident",
          "nqid": 0,
          "ao": 2,
          "ica": false
        },
        {
          "aid": 39,
          "at": "Not Sure",
          "nqid": 0,
          "ao": 3,
          "ica": false
        },
        {
          "aid": 38,
          "at": "Not Confident",
          "nqid": 0,
          "ao": 4,
          "ica": false
        }
      ]
    },
    {
      "tqid": 46,
      "qid": 17,
      "qt": "P/G/M/I",
      "qtid": 1,
      "cid": 1,
      "qgid": 3,
      "qpid": 0,
      "qo": 1,
      "a": [
        {
          "aid": 33,
          "at": "P",
          "nqid": 0,
          "ao": 1,
          "ica": false
        },
        {
          "aid": 34,
          "at": "G",
          "nqid": 0,
          "ao": 2,
          "ica": false
        },
        {
          "aid": 35,
          "at": "M",
          "nqid": 0,
          "ao": 3,
          "ica": false
        },
        {
          "aid": 36,
          "at": "I",
          "nqid": 0,
          "ao": 4,
          "ica": true
        }
      ]
    },
    {
      "tqid": 47,
      "qid": 18,
      "qt": "Reasons for your answer:",
      "qtid": 3,
      "cid": 1,
      "qgid": 3,
      "qpid": 46,
      "qo": 2,
      "a": [
        {
          "aid": 42,
          "at": "Minor skin folds",
          "nqid": 0,
          "ao": 1,
          "ica": false
        },
        {
          "aid": 43,
          "at": "Minor artefact but not impacting on tissue visualization",
          "nqid": 0,
          "ao": 2,
          "ica": false
        },
        {
          "aid": 44,
          "at": "Minor asymmetrical images",
          "nqid": 0,
          "ao": 3,
          "ica": false
        },
        {
          "aid": 45,
          "at": "Pectoral muscle not to nipple level but posterior breast tissue adequately shown",
          "nqid": 0,
          "ao": 4,
          "ica": false
        },
        {
          "aid": 46,
          "at": "Nipple not in profile but retro-areolar tissue well demonstrated",
          "nqid": 0,
          "ao": 5,
          "ica": false
        },
        {
          "aid": 47,
          "at": "Infra-mammary fold not clearly demonstrated but breast tissue adequately shown",
          "nqid": 0,
          "ao": 6,
          "ica": false
        },
        {
          "aid": 48,
          "at": "A significant part of the breast not imaged",
          "nqid": 0,
          "ao": 7,
          "ica": false
        },
        {
          "aid": 49,
          "at": "Underexposed",
          "nqid": 0,
          "ao": 8,
          "ica": false
        },
        {
          "aid": 50,
          "at": "Overexposed",
          "nqid": 0,
          "ao": 9,
          "ica": false
        },
        {
          "aid": 51,
          "at": "Inadequate compression hindering diagnosis",
          "nqid": 0,
          "ao": 10,
          "ica": false
        },
        {
          "aid": 52,
          "at": "Blurred image due to movement",
          "nqid": 0,
          "ao": 11,
          "ica": false
        },
        {
          "aid": 53,
          "at": "Incorrect processing",
          "nqid": 0,
          "ao": 12,
          "ica": false
        },
        {
          "aid": 54,
          "at": "Overlying artefacts",
          "nqid": 0,
          "ao": 13,
          "ica": false
        },
        {
          "aid": 55,
          "at": "Skin folds obscuring the image",
          "nqid": 0,
          "ao": 14,
          "ica": true
        }
      ]
    },
    {
      "tqid": 48,
      "qid": 17,
      "qt": "P/G/M/I",
      "qtid": 1,
      "qgid": 3,
      "cid": 2,
      "qpid": 0,
      "qo": 1,
      "a": [
        {
          "aid": 33,
          "at": "P",
          "nqid": 0,
          "ao": 1,
          "ica": false
        },
        {
          "aid": 34,
          "at": "G",
          "nqid": 0,
          "ao": 2,
          "ica": false
        },
        {
          "aid": 35,
          "at": "M",
          "nqid": 0,
          "ao": 3,
          "ica": true
        },
        {
          "aid": 36,
          "at": "I",
          "nqid": 0,
          "ao": 4,
          "ica": false
        }
      ]
    },
    {
      "tqid": 49,
      "qid": 18,
      "qt": "Reasons for your answer:",
      "qtid": 3,
      "qgid": 3,
      "cid": 2,
      "qpid": 48,
      "qo": 2,
      "a": [
        {
          "aid": 42,
          "at": "Minor skin folds",
          "nqid": 0,
          "ao": 1,
          "ica": false
        },
        {
          "aid": 43,
          "at": "Minor artefact but not impacting on tissue visualization",
          "nqid": 0,
          "ao": 2,
          "ica": false
        },
        {
          "aid": 44,
          "at": "Minor asymmetrical images",
          "nqid": 0,
          "ao": 3,
          "ica": false
        },
        {
          "aid": 45,
          "at": "Pectoral muscle not to nipple level but posterior breast tissue adequately shown",
          "nqid": 0,
          "ao": 4,
          "ica": false
        },
        {
          "aid": 46,
          "at": "Nipple not in profile but retro-areolar tissue well demonstrated",
          "nqid": 0,
          "ao": 5,
          "ica": false
        },
        {
          "aid": 47,
          "at": "Infra-mammary fold not clearly demonstrated but breast tissue adequately shown",
          "nqid": 0,
          "ao": 6,
          "ica": false
        },
        {
          "aid": 48,
          "at": "A significant part of the breast not imaged",
          "nqid": 0,
          "ao": 7,
          "ica": false
        },
        {
          "aid": 49,
          "at": "Underexposed",
          "nqid": 0,
          "ao": 8,
          "ica": false
        },
        {
          "aid": 50,
          "at": "Overexposed",
          "nqid": 0,
          "ao": 9,
          "ica": false
        },
        {
          "aid": 51,
          "at": "Inadequate compression hindering diagnosis",
          "nqid": 0,
          "ao": 10,
          "ica": false
        },
        {
          "aid": 52,
          "at": "Blurred image due to movement",
          "nqid": 0,
          "ao": 11,
          "ica": false
        },
        {
          "aid": 53,
          "at": "Incorrect processing",
          "nqid": 0,
          "ao": 12,
          "ica": false
        },
        {
          "aid": 54,
          "at": "Overlying artefacts",
          "nqid": 0,
          "ao": 13,
          "ica": true
        },
        {
          "aid": 55,
          "at": "Skin folds obscuring the image",
          "nqid": 0,
          "ao": 14,
          "ica": false
        }
      ]
    },
    {
      "tqid": 50,
      "qid": 17,
      "qt": "P/G/M/I",
      "qtid": 1,
      "cid": 3,
      "qgid": 3,
      "qpid": 0,
      "qo": 1,
      "a": [
        {
          "aid": 33,
          "at": "P",
          "nqid": 0,
          "ao": 1,
          "ica": false
        },
        {
          "aid": 34,
          "at": "G",
          "nqid": 0,
          "ao": 2,
          "ica": true
        },
        {
          "aid": 35,
          "at": "M",
          "nqid": 0,
          "ao": 3,
          "ica": false
        },
        {
          "aid": 36,
          "at": "I",
          "nqid": 0,
          "ao": 4,
          "ica": false
        }
      ]
    },
    {
      "tqid": 51,
      "qid": 18,
      "qt": "Reasons for your answer:",
      "qtid": 3,
      "qgid": 3,
      "cid": 3,
      "qpid": 50,
      "qo": 2,
      "a": [
        {
          "aid": 42,
          "at": "Minor skin folds",
          "nqid": 0,
          "ao": 1,
          "ica": false
        },
        {
          "aid": 43,
          "at": "Minor artefact but not impacting on tissue visualization",
          "nqid": 0,
          "ao": 2,
          "ica": false
        },
        {
          "aid": 44,
          "at": "Minor asymmetrical images",
          "nqid": 0,
          "ao": 3,
          "ica": false
        },
        {
          "aid": 45,
          "at": "Pectoral muscle not to nipple level but posterior breast tissue adequately shown",
          "nqid": 0,
          "ao": 4,
          "ica": false
        },
        {
          "aid": 46,
          "at": "Nipple not in profile but retro-areolar tissue well demonstrated",
          "nqid": 0,
          "ao": 5,
          "ica": false
        },
        {
          "aid": 47,
          "at": "Infra-mammary fold not clearly demonstrated but breast tissue adequately shown",
          "nqid": 0,
          "ao": 6,
          "ica": false
        },
        {
          "aid": 48,
          "at": "A significant part of the breast not imaged",
          "nqid": 0,
          "ao": 7,
          "ica": false
        },
        {
          "aid": 49,
          "at": "Underexposed",
          "nqid": 0,
          "ao": 8,
          "ica": false
        },
        {
          "aid": 50,
          "at": "Overexposed",
          "nqid": 0,
          "ao": 9,
          "ica": false
        },
        {
          "aid": 51,
          "at": "Inadequate compression hindering diagnosis",
          "nqid": 0,
          "ao": 10,
          "ica": false
        },
        {
          "aid": 52,
          "at": "Blurred image due to movement",
          "nqid": 0,
          "ao": 11,
          "ica": false
        },
        {
          "aid": 53,
          "at": "Incorrect processing",
          "nqid": 0,
          "ao": 12,
          "ica": true
        },
        {
          "aid": 54,
          "at": "Overlying artefacts",
          "nqid": 0,
          "ao": 13,
          "ica": false
        },
        {
          "aid": 55,
          "at": "Skin folds obscuring the image",
          "nqid": 0,
          "ao": 14,
          "ica": false
        }
      ]
    },
    {
      "tqid": 52,
      "qid": 17,
      "qt": "P/G/M/I",
      "qtid": 1,
      "cid": 4,
      "qgid": 3,
      "qpid": 0,
      "qo": 1,
      "a": [
        {
          "aid": 33,
          "at": "P",
          "nqid": 0,
          "ao": 1,
          "ica": true
        },
        {
          "aid": 34,
          "at": "G",
          "nqid": 0,
          "ao": 2,
          "ica": false
        },
        {
          "aid": 35,
          "at": "M",
          "nqid": 0,
          "ao": 3,
          "ica": false
        },
        {
          "aid": 36,
          "at": "I",
          "nqid": 0,
          "ao": 4,
          "ica": false
        }
      ]
    },
    {
      "tqid": 53,
      "qid": 18,
      "qt": "Reasons for your answer:",
      "qtid": 3,
      "qgid": 3,
      "cid": 4,
      "qpid": 52,
      "qo": 2,
      "a": [
        {
          "aid": 42,
          "at": "Minor skin folds",
          "nqid": 0,
          "ao": 1,
          "ica": false
        },
        {
          "aid": 43,
          "at": "Minor artefact but not impacting on tissue visualization",
          "nqid": 0,
          "ao": 2,
          "ica": false
        },
        {
          "aid": 44,
          "at": "Minor asymmetrical images",
          "nqid": 0,
          "ao": 3,
          "ica": false
        },
        {
          "aid": 45,
          "at": "Pectoral muscle not to nipple level but posterior breast tissue adequately shown",
          "nqid": 0,
          "ao": 4,
          "ica": false
        },
        {
          "aid": 46,
          "at": "Nipple not in profile but retro-areolar tissue well demonstrated",
          "nqid": 0,
          "ao": 5,
          "ica": false
        },
        {
          "aid": 47,
          "at": "Infra-mammary fold not clearly demonstrated but breast tissue adequately shown",
          "nqid": 0,
          "ao": 6,
          "ica": false
        },
        {
          "aid": 48,
          "at": "A significant part of the breast not imaged",
          "nqid": 0,
          "ao": 7,
          "ica": false
        },
        {
          "aid": 49,
          "at": "Underexposed",
          "nqid": 0,
          "ao": 8,
          "ica": false
        },
        {
          "aid": 50,
          "at": "Overexposed",
          "nqid": 0,
          "ao": 9,
          "ica": true
        },
        {
          "aid": 51,
          "at": "Inadequate compression hindering diagnosis",
          "nqid": 0,
          "ao": 10,
          "ica": false
        },
        {
          "aid": 52,
          "at": "Blurred image due to movement",
          "nqid": 0,
          "ao": 11,
          "ica": false
        },
        {
          "aid": 53,
          "at": "Incorrect processing",
          "nqid": 0,
          "ao": 12,
          "ica": false
        },
        {
          "aid": 54,
          "at": "Overlying artefacts",
          "nqid": 0,
          "ao": 13,
          "ica": false
        },
        {
          "aid": 55,
          "at": "Skin folds obscuring the image",
          "nqid": 0,
          "ao": 14,
          "ica": false
        }
      ]
    }
  ],
  "c": [
    {
      "cid": 1,
      "cellr": 1,
      "celll": 1,
      "cimg": "ee.com",
      "cpjson": "[\n    {\n        \"name\": \"LCC\",\n        \"value\": \"Left Craniocaudal\"\n    }\n]",
      "acpjson": "1e"
    },
    {
      "cid": 2,
      "cellr": 1,
      "celll": 2,
      "cimg": "ff.com",
      "cpjson": "[\n    {\n        \"name\": \"RCC\",\n        \"value\": \"Right Craniocaudal\"\n    }\n]",
      "acpjson": "2f"
    },
    {
      "cid": 3,
      "cellr": 2,
      "celll": 1,
      "cimg": "gg.com",
      "cpjson": "[\n    {\n        \"name\": \"LMLO\",\n        \"value\": \"Left Medio-lateral Oblique\"\n    }\n]",
      "acpjson": "3g"
    },
    {
      "cid": 4,
      "cellr": 2,
      "celll": 2,
      "cimg": "hh.com",
      "cpjson": "[\n    {\n        \"name\": \"RMLO\",\n        \"value\": \"Right Medio-lateral Oblique\"\n    }\n]",
      "acpjson": "4h"
    }
  ]
}

export default data;