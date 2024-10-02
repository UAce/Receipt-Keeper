CREATE TABLE IF NOT EXISTS "User" (
    "Id" uuid NOT NULL DEFAULT gen_random_uuid (),
    "FirstName" varchar(50) NOT NULL,
    "LastName" varchar(50) NOT NULL,
    "Email" varchar(320) NOT NULL,
    "IdentityId" varchar(36) NOT NULL,
    -- Timestamp columns
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT now(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT now(),
    "DeletedAt" TIMESTAMP WITH TIME ZONE,
    -- Constraints
    CONSTRAINT "user_id_pkey" PRIMARY KEY ("Id")
);

-- We create indices to speed up queries that search by Id or IdentityId
CREATE UNIQUE INDEX IF NOT EXISTS "user_id_idx" ON "User" ("Id");

CREATE UNIQUE INDEX IF NOT EXISTS "user_identity_id_idx" ON "User" ("IdentityId");
