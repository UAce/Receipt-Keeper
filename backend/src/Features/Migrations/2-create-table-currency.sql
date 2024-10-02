CREATE TABLE IF NOT EXISTS "Currency" (
  "Code" varchar(3) NOT NULL,
  "Name" varchar(50) NOT NULL,
  "Flag" text NOT NULL, -- emoji can have variable length
  -- Constraints
  CONSTRAINT "currency_code_pkey" PRIMARY KEY ("Code")
);

-- We create an index to speed up queries that search by Code
CREATE UNIQUE INDEX IF NOT EXISTS "currency_code_idx" ON "Currency" ("Code");